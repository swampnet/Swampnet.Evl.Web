Vue.component('expression', {
    props: {
        exp: Object,
        parent: Object,
        rule: Object
    },
    methods: {
        isContainer() {
            return (this.exp.operator == 'MATCH_ALL' || this.exp.operator == 'MATCH_ANY');
        },
        addChild() {
            var e = {
                id: uuidv4(),
                operand: 'Summary',
                operator: 'EQ',
                children: []
            };
            this.exp.children.push(e);
        },
        deleteExpression(e) {
            if (this.parent) {
                var index = this.parent.children.indexOf(e);
                if (index > -1) {
                    this.parent.children.splice(index, 1);
                }
            }
        }
    },
    template: `
    <div v-if="exp">
      <div v-if="isContainer()" class="evt-detail-expression-container">
        <select v-model="exp.operator">
          <option v-for="option in rule.metaData.operators" v-bind:value="option.code">
            {{ option.display }}
          </option>
        </select>

        <button v-if="parent" v-on:click="deleteExpression(exp)" class="evt-detail-expression-delete">x</button>
        <expression v-for="child in exp.children" v-bind:exp="child" v-bind:parent="exp" v-bind:rule="rule" v-bind:key="child.key"></expression>
        
        <button v-on:click="addChild()">add</button>
      </div>


      <div v-else class="evt-detail-expression-child">
        <select v-model="exp.operand">
          <option v-for="option in rule.metaData.operands" v-bind:value="option.name">
            {{ option.name }}
          </option>
        </select>

        <input v-if="exp.operand=='Property'" v-model="exp.argument">

        <select v-model="exp.operator">
          <option v-for="option in rule.metaData.operators" v-bind:value="option.code">
            {{ option.display }}
          </option>
        </select>

        <input v-model="exp.value">
        <button v-on:click="deleteExpression(exp)" class="evt-detail-expression-delete">x</button>
      </div>

    </div>
`
})


var app = new Vue({
    el: '#rules',
    data: {
        id: '',
        rule: null
    },
    methods: {
        async save(e) {
            try {
                await axios.put('', this.rule);
            } catch (error) {
                console.log(error);
            }
        },
        assignKey(e) {
            if (e != null) {
                e.forEach(x => {
                    x.key = uuidv4();
                });
                this.assignKey(e.children);
            }
        }
    },
    async created() {
        this.id = window.location.href.substring(window.location.href.lastIndexOf('/') + 1);

        try {
            const rs = await axios.get(this.id + '/json');
            var rule = rs.data;

            rule.expression.key = uuidv4();
            this.assignKey(rule.expression.children);

            this.rule = rule;
        } catch (error) {
            console.log(error);
        }
    }
})