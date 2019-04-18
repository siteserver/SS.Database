var $api = axios.create({
  baseURL: utils.getQueryString('apiUrl') + '/' + utils.getQueryString('pluginId') + '/pages/query/',
  withCredentials: true
});

var data = {
  siteId: utils.getQueryInt('siteId'),
  advertisementType: utils.getQueryString('advertisementType'),
  pageLoad: false,
  pageAlert: null,
  query: '',
  dataInfoList: null,
  properties: null,
  count: 0
};

var methods = {
  apiQuery: function () {
    var $this = this;

    utils.loading(true);
    $api.post('', {
      query: this.query
    }).then(function (response) {
      var res = response.data;

      $this.dataInfoList = res.value;
      $this.properties = res.properties;
      $this.count = res.count;

      $this.pageAlert = {
        type: 'success',
        html: '查询成功，共返回 ' + $this.count + ' 条结果。'
      };
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      utils.loading(false);
    });
  },

  getColumnAttributes: function(columnInfo) {
    var retval = '';
    if(columnInfo.isIdentity) retval += ", 自增长";
    if(columnInfo.isPrimaryKey) retval += ", 主键";
    if(columnInfo.isExtend) retval += ", 扩展字段";
    if(retval)
    {
      retval = retval.substr(2);
    }
    return retval;
  },

  btnNavClick: function (pageName) {
    location.href = utils.getPageUrl(pageName);
  },

  btnSubmitClick: function () {
    var $this = this;
    this.pageAlert = null;

    this.$validator.validate().then(function (result) {
      if (result) {
        $this.apiQuery();
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
