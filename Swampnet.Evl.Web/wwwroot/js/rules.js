var app = new Vue({
    el: '#rules',
    data: {
        message: 'Hello Vue!',
        rules: []
    },
    async created() {
        console.log('created');

        try {
            const rs = await axios.get('rules/json');
            this.rules = rs.data;
        } catch (e) {
            console.log(e);
        }
    }
})