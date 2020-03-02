/**
 * 获取惟一Id号
 */
export function getId(length = 5) {
  return Number(
    Math.random()
      .toString()
      .substr(3, length) + Date.now()
  ).toString(16);
}

/**
 * datagrid 客户端分页函数
 * @param data
 */
export function pagefilter(data) {
  if ($.isArray(data)) {
    data = {
      total: data.length,
      rows: data
    };
  }
  let $dg = <any>$(this);
  let opts = $dg.datagrid("options");
  let pager = $dg.datagrid("getPager");
  pager.pagination({
    onSelectPage: function(pageNum, pageSize) {
      opts.pageNumber = pageNum;
      opts.pageSize = pageSize;
      pager.pagination("refresh", {
        pageNumber: pageNum,
        pageSize: pageSize
      });
      $dg.datagrid("loadData", data);
    }
  });
  if (!data.originalRows) {
    //保存原始记录
    data.originalRows = data.rows;
  }
  let start = (opts.pageNumber - 1) * parseInt(opts.pageSize);
  let end = start + parseInt(opts.pageSize);
  data.rows = data.originalRows.slice(start, end);
  return data;
}

/**
 * 状态颜色值，从低->高
 * 最高级别是联动报警
 */
export function getStatusColorLevel(){
  return {
    'norm': '#00cc66',
    'alarm': [ '#66ccff', '#0000ff', '#ff6633', '#cc0000' ],
    'union': '#cc9933'
  }
}

export function getStatusColorLevelSlope(){
  return {
    'norm': '#0f0',
    'alarm': [ '#ff0000', '#f6ba40', '#f7f910', '#0000fe','#585858' ]
  }
}

/**
 * 注册组件
 * @param name 
 * @param config 
 */
export function registerCompoent(name:string, config:any){
  ko.components.register(name, config);
}
