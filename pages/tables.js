var $api = axios.create({
  baseURL: utils.getQueryString('apiUrl') + '/' + utils.getQueryString('pluginId') + '/pages/tables/',
  withCredentials: true
});

var data = {
  siteId: utils.getQueryInt('siteId'),
  advertisementType: utils.getQueryString('advertisementType'),
  pageLoad: false,
  pageAlert: null,
  columnInfoList: null,
  types: null,
  tableNames: null,
  table: '',
  count: 0
};

var methods = {
  apiGetTableNameList: function () {
    var $this = this;

    if ($this.pageLoad) utils.loading(true);
    $api.get('').then(function (response) {
      var res = response.data;

      $this.tableNames = res.value;
      $this.types = res.types;
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      $this.pageLoad = true;
      utils.loading(false);
    });
  },

  apiGetTableColumnInfoList: function () {
    var $this = this;

    utils.loading(true);
    $api.post('', {
      tableName: this.table
    }).then(function (response) {
      var res = response.data;

      $this.columnInfoList = res.value;
      $this.count = res.count;
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      utils.loading(false);
    });
  },

  btnNavClick: function (pageName) {
    location.href = utils.getPageUrl(pageName);
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
  }
};

new Vue({
  el: '#main',
  data: data,
  watch: {
    table: function (val, oldVal) {
      this.apiGetTableColumnInfoList();
    }
  },
  methods: methods,
  created: function () {
    this.apiGetTableNameList();
  }
});
