var $api = axios.create({
    baseURL: utils.getQueryString('apiUrl') + '/' + utils.getQueryString('pluginId') + '/pages/execute/',
    withCredentials: true
  });
  
  var data = {
    siteId: utils.getQueryInt('siteId'),
    advertisementType: utils.getQueryString('advertisementType'),
    pageLoad: false,
    pageAlert: null,
    execute: '',
    secretKey: ''
  };
  
  var methods = {
    apiExecute: function () {
      var $this = this;
  
      utils.loading(true);
      $api.post('', {
        execute: this.execute,
        secretKey: this.secretKey
      }).then(function (response) {
        $this.pageAlert = {
          type: 'success',
          html: 'SQL命令执行成功！'
        };
      }).catch(function (error) {
        $this.pageAlert = utils.getPageAlert(error);
      }).then(function () {
        utils.loading(false);
      });
    },
  
    btnNavClick: function (pageName) {
      location.href = utils.getPageUrl(pageName);
    },
  
    btnSubmitClick: function () {
      var $this = this;
      this.pageAlert = null;
  
      this.$validator.validate().then(function (result) {
        if (result) {
          $this.apiExecute();
        }
      });
    }
  };
  
  new Vue({
    el: '#main',
    data: data,
    methods: methods,
    created: function () {
      this.pageLoad = true;
    }
  });
  