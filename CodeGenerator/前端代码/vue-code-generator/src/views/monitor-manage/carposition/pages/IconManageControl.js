const obj = {
  icons01: [
    ['czc_1000', 'czc_1499'],
    ['gjc_1500', 'gjc_1999'],
    ['hc_che_2000', 'hc_che_2499'],
    ['kc_2500', 'kc_2999'],
    ['wh_gt_3000', 'wh_gt_3003'],
    ['wh_yt_4000', 'wh_yt_4003'],
    ['wh_qt_4500', 'wh_qt_4503'],
    ['wh_yrw_5000', 'wh_yrw_5003'],
    ['wh_qt_5500', 'wh_qt_5503'],
    ['bzc_6000'],
    ['dc_6500'],
    ['jc_7000'],
    ['xcc_7500'],
    ['hc_chuan_8000'],
    ['lyct_8500'],
    ['zfc_9000'],
    ['qt_9500', 'qt_9999']
  ],
  icons02: [
    ['wh_gt_3004', 'wh_gt_3499'],
    ['wh_yt_4004', 'wh_yt_4499'],
    ['wh_qt_4504', 'wh_qt_4999'],
    ['wh_yrw_5004', 'wh_yrw_5499'],
    ['wh_qt_5504', 'wh_qt_5999'],
    ['bzc_6001', 'bzc_6499'],
    ['dc_6501', 'dc_6999'],
    ['jc_7001', 'jc_7499'],
    ['xcc_7501', 'xcc_7999'],
    ['hc_chuan_8001', 'hc_chuan_8499'],
    ['lyct_8501', 'lyct_8999'],
    ['zfc_9001', 'zfc_9499']
  ],
  getIconClass(icon) {
    const fn = (e) =>
      e.length == 2
        ? icon.localeCompare(e[0]) >= 0 && icon.localeCompare(e[1]) <= 0
        : icon === e[0]
    if (this.icons01.some(fn)) {
      return 'track-icon icons01 ' + icon
    } else if (this.icons02.some(fn)) {
      return 'track-icon icons02 ' + icon
    } else {
      return 'track-icon ' + icon
    }
  }
}
export default obj

