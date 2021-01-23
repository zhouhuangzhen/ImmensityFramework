import helperDate from './datehelper.js'
import helperConvert from './convert.js'
import helperEchart from './echarthelper.js'
import helperCommunicate from './communicate.js'
import { userinfo, baseinfo } from './infohelper.js'
import helperGenerate from './generatehelper.js'

const helpers = {
  date: helperDate,
  convert: helperConvert,
  echart: helperEchart,
  communicate: helperCommunicate,
  info: { baseinfo, userinfo },
  generate: helperGenerate
}
export default helpers
