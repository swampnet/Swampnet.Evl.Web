Vue.component('expression', {
    props: ['exp'],
    template: `
      <div v-if="exp" class="evt-detail-expressions">
        <input v-model="exp.operand"> <input v-model="exp.operator"> <input v-model="exp.value">
        <expression v-for="child in exp.children" v-bind:exp="child" v-bind:key="child.key"></expression>
      </div>`
})


//function assignKey(e) {
//    if (e != null) {
//        e.forEach(x => {
//            x.key = uuidv4();
//        });
//        assignKey(e.children);
//    }
//}


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