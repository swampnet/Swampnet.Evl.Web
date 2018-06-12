var app = new Vue({
    el: '#rules',
    data: {
        id: '',
        rule: null
    },
    async created() {
        this.id = window.location.href.substring(window.location.href.lastIndexOf('/') + 1);

        try {
            const rs = await axios.get(this.id + '/json');
            this.rule = rs.data;
        } catch (e) {
            console.log(e);
        }
    }
})