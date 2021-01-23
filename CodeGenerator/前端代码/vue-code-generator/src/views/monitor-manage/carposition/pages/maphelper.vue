<template>
  <div id="map" ref="map" />
</template>

<script>
import minemap from 'minemap'
import TrackPlaybackControl from './track'

export default {
  props: {
    /**
     * domainUrl : http://mapsjgt.sz-its.cn
     * spriteUrl :
     * serviceUrl :
     * token :
     * solution :
     * features :
     * center :
     * minZoom :
     * maxZoom :
     * zoom :
     * pitch :
     */
    options: Object
  },
  data() {
    return {
      map: null,
      isMapLoad: false,
      trackplay: null
    }
  },
  computed: {
    changeMapbyDark() {
      return this.map.setStyle(
        '//minedata.cn/service/solu/style/id/' + (this.dark ? 2374 : 2365)
      )
    }
  },
  watch: {
    isMapLoad: function(newValue, oldValue) {
      this.isMapLoad = this.map.loaded()
    },
    features: function(newValue, oldValue) {
      this.map
        .getSource('LineStringSource')
        .setData({ type: 'FeatureCollection', features: [] })
      this.map
        .getSource('PointSource')
        .setData({ type: 'FeatureCollection', features: [] })
      this.map
        .getSource('PolygonSource')
        .setData({ type: 'FeatureCollection', features: [] })
      const json = newValue
      const layerType = json.features[0].geometry.type

      this.map.getSource(layerType + 'Source').setData(json)
    }
  },
  mounted() {
    this.initMap()
  },
  methods: {
    //  初始化地图
    initMap() {
      const conf_domainUrl = this.options.domainUrl ?? 'http://mapsjgt.sz-its.cn'
      minemap.domainUrl = conf_domainUrl
      minemap.dataDomainUrl = conf_domainUrl
      minemap.spriteUrl = conf_domainUrl + '/minemapapi/v2.0.0/sprite/sprite'
      minemap.serviceUrl = conf_domainUrl + '/service'
      minemap.accessToken = !this.options.token ? 'ab86503d8ec44480aa48083d25c73902' : this.options.token
      this.map = new minemap.Map({
        container: 'map',
        style: minemap.serviceUrl + '/solu/style/id/' + (!this.solution ? '4755' : this.solution),
        center: !this.center ? [120.60958, 31.33135] : this.center,
        minZoom: !this.minZoom ? 8 : this.minZoom,
        maxZoom: !this.maxZoom ? 16 : this.maxZoom,
        zoom: !this.zoom ? 10 : this.zoom,
        pitch: !this.pitch ? 0 : this.pitch,
        logoControl: false /* logo控件是否显示，不加该参数时默认显示*/
      })

      this.map.on('load', () => {
        this.isMapLoad = true
        this.mapLoaded()
      // this.$emit('loaded', { map: this.map })
      })

      this.trackplay = new TrackPlaybackControl(this.map, {
        autoPlay: false,
        // controlBarPosition: "top-left"
        controlBarPosition: 'top-right'
      //    controlBarPosition: "bottom-right",
      //    controlBarPosition: "bottom-left"
      })
    },
    //  地图变化
    changeMap() {
      this.map.setStyle('http://mapsjgt.sz-its.cn/service/solu/style/id/4662')
    },
    //  地图加载
    mapLoaded() {
      if (this.map.loaded() != true) return

      this.map.addSource('LineStringSource', {
        type: 'geojson',
        data: null
      })
      this.map.addLayer({
        id: 'line',
        type: 'line',
        source: 'LineStringSource',
        paint: {
          'line-color': 'rgba(0, 239, 253,0.4)',

          'line-width': 2
        }
      })
      // areaLine
      this.map.addSource('PointSource', {
        type: 'geojson',
        data: null
      })
      this.map.addLayer({
        id: 'point',
        type: 'symbol',
        source: 'PointSource',
        layout: {
          'icon-image': 'circle-red-11', // circle-red-11(圆点图)  bus-15(公交图)   metro-1-230100-18(换乘图)
          'icon-size': 1
        }
      })
      this.map.addSource('PolygonSource', {
        type: 'geojson',
        data: null
      })
      this.map.addLayer({
        id: 'polygon',
        type: 'fill',
        source: 'PolygonSource',
        layout: {},
        paint: {
          'fill-color': 'rgba(0, 239, 253,0.4)',
          'fill-opacity': 0.8
        }
      })
    },
    //  轨迹回放方法--开始
    /**
   * 设置轨迹回放数据
   */
    setData_HisLocation(data) {
      this.trackplay.setData_HisLocation(data)
    },
    /**
   * 设置轨迹回放显示方式
   */
    setData_HisLocationDisplay(data) {
      this.trackplay.setData_HisLocationDisplay(data)
    },
    /**
   * 设置取消或暂停轨迹回放
   */
    setData_CancelHisLocation(data) {
      this.trackplay.setData_CancelHisLocation(data)
    },
    /**
   * 设置显示或隐藏轨迹回放控制条
   */
    setData_DisplayControlBar(data) {
      this.trackplay.setData_DisplayControlBar(data)
    },
    /**
   * 设置工具条样式
   */
    setData_ControlBarStyle(data) {
      this.trackplay.setData_ControlBarStyle(data)
    },
    /**
   * 设置弹出窗样式
   */
    setData_PopupStyle(data) {
      this.trackplay.setData_PopupStyle(data)
    },
    /**
 * 设置轨迹回放回调函数
 */
    setData_CallBack_HisLocationPoint(cb) {
      this.trackplay.setData_CallBack_HisLocationPoint(cb)
    },
    /**
   * 返回取消或暂停轨迹回放
   */
    setData_CallBack_CancelHisLocation(cb) {
      this.trackplay.setData_CallBack_CancelHisLocation(cb)
    }
  //  轨迹回放方法--结束
  //  定位
  }
}
</script>

<style lang="scss" scoped>
/* @import "../plugins/mineMap/minemap.css";
@import "../plugins/mineMap/minemap-dark.css"; */
#map {
  width: 100%;
  height: 100%;
   position: relative;
  z-index: 10;
}
</style>
