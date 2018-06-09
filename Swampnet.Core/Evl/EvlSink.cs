﻿using Serilog.Sinks.PeriodicBatching;
using System;
using System.Collections.Generic;
using System.Text;
using Serilog.Events;
using Serilog.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Serilog.Debugging;
using System.Diagnostics;
using System.Reflection;
using Swampnet.Core.Evl;
using System.Net.Http;
using Newtonsoft.Json;

namespace Serilog.Sinks.Evl
{
    public class EvlSink : PeriodicBatchingSink
    {
		public const string CATEGORY_SPLIT = "~CAT~";
		public const string TAG_CATEGORY = "~TAG~";
		public const string ID = "~ID~";


		private static readonly int _defaultBatchSize = 50;                        // Maximum number of LogEvents in a batch
        private static readonly TimeSpan _defaultPeriod = TimeSpan.FromSeconds(5); // How often we flush the batch

        private readonly IFormatProvider _formatProvider;
        private readonly List<Event> _failedEvents = new List<Event>();
        private readonly string _source;
        private readonly string _sourceVersion;
        private readonly string _apiKey;
        private readonly string _endpoint;
        private readonly HttpClient _client;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="formatProvider"></param>
        /// <param name="apiKey"></param>
        /// <param name="endpoint"></param>
        /// <param name="source">Event source. Defaults to executing assembly name</param>
        /// <param name="sourceVersion">Event source version. Defaults to executing assembly version</param>
        public EvlSink(IFormatProvider formatProvider, string apiKey, string endpoint, string source, string sourceVersion)
            : this(_defaultBatchSize, _defaultPeriod)
        {
            _source = source;
            _sourceVersion = sourceVersion;
            _apiKey = apiKey;
            _endpoint = endpoint;

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("x-api-key", _apiKey);

            if (string.IsNullOrEmpty(_source))
            {
                _source = "unknown";
                // Need to bump to .net standard 1.5 for this
                //var ass = Assembly.GetEntryAssembly();
                //if(ass != null)
                //{
                //    var name = ass.GetName();
                //    if(name != null)
                //    {
                //        source = name.Name;
                //        sourceVersion = name.Version.ToString();
                //    }
                //}
            }

            _formatProvider = formatProvider;
        }


        protected EvlSink(int batchSizeLimit, TimeSpan period) 
            : base(batchSizeLimit, period)
        {
        }


        protected EvlSink(int batchSizeLimit, TimeSpan period, int queueLimit) 
            : base(batchSizeLimit, period, queueLimit)
        {
        }



        protected override async Task EmitBatchAsync(IEnumerable<LogEvent> events)
        {
            var evlEvents = Convert(events);

            try
            {
                await PostAsync(_failedEvents.Concat(evlEvents));

                _failedEvents.Clear();
            }
            catch (Exception ex)
            {
                SelfLog.WriteLine("Unable to write {0} log events due to following error: {1}", events.Count(), ex.Message);

                // @TODO: Should probably do something here to stop this growing out of control...
                _failedEvents.AddRange(evlEvents);

                throw;
            }
        }


        protected virtual async Task PostAsync(IEnumerable<Event> events)
        {
            var rs = await _client.PostAsync(
                    _endpoint + "/events/bulk",
                    new StringContent(
                        JsonConvert.SerializeObject(events),
                        Encoding.UTF8,
                        "application/json"));

            rs.EnsureSuccessStatusCode();
        }



        // Convert Serilog LogEvent to an Evl.Event
        protected IEnumerable<Event> Convert(IEnumerable<LogEvent> source)
        {
            var evlEvents = new List<Event>();

            foreach(var s in source)
            {
                var evlEvent = new Event();
                evlEvent.Source = _source;
                evlEvent.SourceVersion = _sourceVersion;
                evlEvent.Summary = s.RenderMessage(_formatProvider);
                evlEvent.TimestampUtc = s.Timestamp.UtcDateTime;
                evlEvent.Category = Convert(s.Level);

				var properties = new List<Property>();

                Process(properties, s.Properties);

				evlEvent.Properties = properties.Where(p => p.Category != TAG_CATEGORY && p.Name != TAG_CATEGORY).ToList();
				evlEvent.Tags = properties.Where(p => p.Category == TAG_CATEGORY && p.Name == TAG_CATEGORY).Select(p => p.Value).ToList();

				evlEvents.Add(evlEvent);
            }

            return evlEvents;
        }

        private EventCategory Convert(LogEventLevel level)
        {
            EventCategory cat = EventCategory.Information;

            switch (level)
            {
                case LogEventLevel.Information:
                    cat = EventCategory.Information;
                    break;

                case LogEventLevel.Fatal:
                case LogEventLevel.Error:
                    cat = EventCategory.Error;
                    break;

                case LogEventLevel.Debug:
                    cat = EventCategory.Debug;
                    break;

                case LogEventLevel.Warning:
                    cat = EventCategory.Warning;
                    break;

                default:
                    throw new NotSupportedException($"Serilog event category {level} not supported");
            }

            return cat;
        }

        private void Process(List<Property> properties, IReadOnlyDictionary<string, LogEventPropertyValue> logEventValues)
        {
            foreach(var logEventValue in logEventValues)
            {
                var scalar = logEventValue.Value as ScalarValue;
                if(scalar != null)
                {
					var parts = Split(logEventValue.Key);

					properties.Add(new Property(parts.Item1, parts.Item2, scalar.Value));
				}

                var d = logEventValue.Value as DictionaryValue;
                if(d != null)
                {
                    Process(properties, null, d.Elements);
                }

                var seq = logEventValue.Value as SequenceValue;
                // @TODO: Handle SequenceValue's

                var str = logEventValue.Value as StructureValue;
                // @TODO: Handle StructureValue's
            }
        }


		private Tuple<string, string> Split(string name)
		{
			string category = "";

			if (name.Contains(EvlSink.ID))
			{
				name = name.Substring(name.IndexOf(EvlSink.ID) + EvlSink.ID.Length);
			}

			if (name.Contains(EvlSink.CATEGORY_SPLIT))
			{
				category = name.Substring(0, name.IndexOf(EvlSink.CATEGORY_SPLIT));
				name = name.Substring(name.IndexOf(EvlSink.CATEGORY_SPLIT) + EvlSink.CATEGORY_SPLIT.Length);
			}

			return new Tuple<string, string>(category, name);
		}


        private void Process(List<Property> properties, string category, IReadOnlyDictionary<ScalarValue, LogEventPropertyValue> logEventValues)
        {
            foreach (var logEventValue in logEventValues)
            {
                var scalar = logEventValue.Value as ScalarValue;
                if (scalar != null)
                {
                    properties.Add(new Property(category, logEventValue.Key.ToString(), scalar.Value));
                }

                var d = logEventValue.Value as DictionaryValue;
                if (d != null)
                {
                    Process(properties, logEventValue.Key.ToString(), d.Elements);
                }

                var seq = logEventValue.Value as SequenceValue;
                // @TODO: Handle SequenceValue's

                var str = logEventValue.Value as StructureValue;
                // @TODO: Handle StructureValue's
            }
        }
    }
}
