const errorMessage = {
  Commmons: {
    1: '系统异常，请联系管理员'
  },
  DataType: {
    10001: '该编码已存在'
  },
  DataItem: {
    10001: '该系统编码已存在',
    10002: '该默认编码已存在',
    10003: '该企业编码已存在',
    10004: '该明细已下发过企业配置，无法删除'
  },
  //  充电站、桩
  ChargingPile: {
    10001: '充电站名和倍率不能同时重复',
    10002: '同一个充电站名不能归属两个部门'
  }
}
export default errorMessage
