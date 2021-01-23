<template>
  <div id="map" ref="map" />
</template>

<script>
import minemap from 'minemap'
import CoordinatesTransform from '@/utils/CoordinatesTransform.js'
import alarmPlay from './AlarmPlayControl.js'
import iconManager from './IconManageControl.js'

export default {
  components: { },
  props: [
    'features',
    'solution',
    'token',
    'center',
    'minZoom',
    'maxZoom',
    'zoom',
    'pitch'
  ],
  data() {
    return {
      map: null,
      isMapLoad: false,
      trackplay: null,
      marker: null, //  需要初始化,小车marker
      popup: null, //  需要初始化,点位弹框
      track: {
        turf: null,
        init: null,
        roadLayerIds: [],
        trackData: null, //  轨迹数据 geojson
        playbackControl: null, //  需要初始化, 播放控件
        speedRate: 1, //  播放速度
        options: {
          // 调用setData_HisLocation时自动开始播放
          autoPlay: false,
          // 小车移出地图时, 地图自动移动保持小车可见
          autoTrack: false,
          // 控件位置
          controlBarPosition: 'top-left',
          // 保存回调函数
          callback: {},
          //  轨迹回放初始化参数
          TrackPlay: {
            PData: {
              Guid: '2b209e5b-f918-4b71-8ddc-775cab200a98',
              Label: '101105',
              CallBack: '1',
              PageStart: '1',
              PageEnd: '1',
              StartIcon: 'd_czc_1000',
              Icon: '042',
              Address: '1'
            },
            LSymbol: [{
              TemplateId: '1',
              LColor: '#0000FF',
              LStyle: 'dash',
              LWight: 3,
              Transpatecy: 0.8,
              DColor: '#FF0000',
              DSize: 10,
              DTranspatecy: 0.5
            }]
          }
        },
        playStep: null, //  小车步进控制
        consts: {
          //  轨迹回放状态常量定义
          PLAYING: 'playing',
          PAUSED: 'paused',
          STOPPED: 'stopped',
          //  缺省模板, 数据中未指定模板时,应用此模板
          DEFAULT_TEMPLATE_ID: 'default',
          //  刷新频率
          FRAME_RATE: 30,
          //  当前播放状态
          playStatus: 'stopped'
        },
        layers: {
          LINE_SOURCE: 'lineSource',
          POINT_SOURCE: 'pointSource',
          LINE_LAYER: 'lineLayer',
          LINE_DASHED_LAYER: 'line_dashed',
          POINT_LAYER: 'pointLayer',
          TRACK_POSITION_SOURCE: 'trackpositionSource',
          TRACK_POSITION_LAYER: 'trackpositionLayer'
        }
      },
      mapStatus: 'position' //  position,track

    }
  },
  computed: {
  },
  watch: {
    // isMapLoad: function(newValue, oldValue) {
    //   this.isMapLoad = this.map.loaded()
    // },
    // features: function(newValue, oldValue) {
    //   this.map
    //     .getSource('LineStringSource')
    //     .setData({ type: 'FeatureCollection', features: [] })
    //   this.map
    //     .getSource('PointSource')
    //     .setData({ type: 'FeatureCollection', features: [] })
    //   this.map
    //     .getSource('PolygonSource')
    //     .setData({ type: 'FeatureCollection', features: [] })
    //   const json = newValue
    //   const layerType = json.features[0].geometry.type

    //   this.map.getSource(layerType + 'Source').setData(json)
    // }
  },
  mounted() {
    this.initMap()
  },
  methods: {
    //  初始化小车
    createMarker() {
      var el = document.createElement('div')
      el.classList.add('icons01')
      el.classList.add('czc_1000')
      el.style.width = '50px'
      el.style.height = '50px'
      el.style.cursor = 'pointer'
      el.style.transform = 'scale(0.8)'
      return new minemap.Marker(el, { offset: [-25, -25] })
    },
    //  初始化地图
    initMap() {
      //  url : http://mapsjgttemp.sz-its.cn
      //  solution : 12655
      //  token : 3048f04bb9f2489cb4857ef118359474
      const conf_domainUrl = 'http://mapsjgt.sz-its.cn'
      minemap.domainUrl = conf_domainUrl
      minemap.dataDomainUrl = conf_domainUrl
      minemap.spriteUrl = conf_domainUrl + '/minemapapi/v2.0.0/sprite/sprite'
      minemap.serviceUrl = conf_domainUrl + '/service'
      minemap.accessToken = !this.token ? 'ab86503d8ec44480aa48083d25c73902' : this.token
      this.map = new minemap.Map({
        container: 'map',
        style: minemap.serviceUrl + '/solu/style/id/' + (!this.solution ? '12669' : this.solution),
        center: !this.center ? [120.60958, 31.33135] : this.center,
        minZoom: !this.minZoom ? 0 : this.minZoom,
        maxZoom: !this.maxZoom ? 17 : this.maxZoom,
        zoom: !this.zoom ? 10 : this.zoom,
        pitch: !this.pitch ? 0 : this.pitch,
        logoControl: false /* logo控件是否显示，不加该参数时默认显示*/
      })

      this.map.on('load', () => {
        this.isMapLoad = true
        this.mapLoaded()
      // this.$emit('loaded', { map: this.map })
      })

      //  this.map = initMap(this.$refs.map)

      //  this.addWindowPosition()
      // this.trackplay = new TrackPlaybackControl(this.map, {
      //   autoPlay: false,
      //   // controlBarPosition: "top-left"
      //   controlBarPosition: 'top-right'
      // //    controlBarPosition: "bottom-right",
      // //    controlBarPosition: "bottom-left"
      // })
    },
    //  初始化地图事件
    initMapEvent() {
      const vm = this
      // 地图点击事件
      this.map.on('click', function(e) {
        var layers = []
        if (vm.map.getLayer(vm.track.layers.POINT_LAYER)) {
          layers.push(vm.track.layers.POINT_LAYER)
        }
        if (vm.map.getLayer(vm.track.layers.TRACK_POSITION_LAYER)) {
          layers.push(vm.track.layers.TRACK_POSITION_LAYER)
        }
        var [feature] = vm.map.queryRenderedFeatures(e.point, {
          layers: layers
        })

        if (!feature) {
          vm.popup.remove()
          return
        }
        var el = document.createElement('div')
        el.className = 'track-popup'
        el.innerHTML = `
                <div><span>车牌号 : ${feature.properties.DBusCard}</span></div>
                <div><span>车辆自编号 : ${feature.properties.BusNo}</span></div>
                <div><span>速度 : ${feature.properties.Speed}</span></div>
                <div><span>时间 : ${feature.properties.Time}</span></div>
                <div><span>${feature.properties.Remark ? feature.properties.Remark : ''}</span></div>
                `

        vm.popup.setLngLat(feature.geometry.coordinates).setDOMContent(el)
        vm.popup.addTo(vm.map)

        // 逆地理编码取得名称
        // const remark = el.querySelector('div:nth-child(2)')
        // remark.innerHTML += `<br><span>&nbsp</span>`
        // vm.getLocationAddress(feature).then((a) => {
        //   remark.querySelector('span:last-child').textContent = a
        // })
      })

      //  鼠标移动事件
      this.map.on('mousemove', function(e) {
        var layers = []
        if (vm.map.getLayer(vm.track.layers.POINT_LAYER)) {
          layers.push(vm.track.layers.POINT_LAYER)
        }
        if (vm.map.getLayer(vm.track.layers.TRACK_POSITION_LAYER)) {
          layers.push(vm.track.layers.TRACK_POSITION_LAYER)
        }
        var features = vm.map.queryRenderedFeatures(e.point, {
          layers: layers
        })
        vm.map.getCanvas().style.cursor = features.length > 0 ? 'pointer' : ''
      })
    },
    //  地图加载
    mapLoaded() {
      if (this.map.loaded() != true) {
        return
      }

      //  初始化marker
      this.marker = this.createMarker()

      //  初始化弹出框对象
      this.popup = new minemap.Popup({
        closeButton: false,
        closeOnClick: false
      })

      //  地图事件初始化
      this.initMapEvent()
    },
    //  地图添加数据
    setData(sourceId, jsonData, lon, lat) {
      var source = this.map.getSource(sourceId)
      if (source) {
        source.setData(jsonData)
      }
      this.map.flyTo({
        center: [lon, lat],
        zoom: 12
      })
    },
    //  添加实时点位地图信息
    addMapInfoPosition() {
      this.removeMapInfoTrack()
      //  Source
      if (!this.map.getSource(this.track.layers.POINT_SOURCE)) {
        this.map.addSource(this.track.layers.POINT_SOURCE, {
          'type': 'geojson',
          'data': null
        })
      }
      //  Layer
      // if (!this.map.getLayer(this.track.layers.POINT_LAYER)) {
      //   this.map.addLayer({
      //     'id': this.track.layers.POINT_LAYER,
      //     'type': 'circle',
      //     'source': this.track.layers.POINT_SOURCE,
      //     'layout': {
      //       'visibility': 'visible'
      //     },
      //     'paint': {
      //       'circle-radius': 10,
      //       'circle-color': {
      //         'type': 'categorical',
      //         'property': 'kind',
      //         'stops': [['click', 'red'], ['other', 'green']],
      //         'default': 'black'
      //       },
      //       'circle-opacity': 0.8
      //     },
      //     'minzoom': 7,
      //     'maxzoom': 17.5
      //   })
      // }
      if (!this.map.getLayer(this.track.layers.POINT_LAYER)) {
        this.map.addLayer({
          'id': this.track.layers.POINT_LAYER,
          'type': 'symbol',
          'source': this.track.layers.POINT_SOURCE,
          'layout': {
            'visibility': 'visible',
            'icon-image': 'marker-15-6',
            'text-field': '{title}',
            'text-offset': [0, 0.6],
            'text-anchor': 'top',
            'text-size': 14,
            'icon-allow-overlap': true, // 图标允许压盖
            'text-allow-overlap': true, // 图标覆盖文字允许压盖
            'icon-size': 1.5
          },
          'paint': {
            'icon-color': {
              'type': 'categorical',
              'property': 'kind',
              'stops': [['click', 'red'], ['other', 'green']],
              'default': 'black'
            },
            'text-color': {
              'type': 'categorical',
              'property': 'kind',
              'stops': [['click', 'red'], ['other', 'green']],
              'default': 'black'
            },
            'text-halo-color': '#000000',
            'text-halo-width': 0.5
          },
          'minzoom': 7,
          'maxzoom': 17.5
        })
      }
    },
    //  删除实时点位地图信息
    removeMapInfoPosition() {
      if (this.map.getSource(this.track.layers.POINT_SOURCE)) {
        this.map.removeSouce(this.track.layers.POINT_SOURCE)
      }
      if (this.map.getLayer(this.track.layers.POINT_LAYER)) {
        this.map.removeLayer(this.track.layers.POINT_LAYER)
      }
    },
    //  添加轨迹回放地图信息
    addMapInfoTrack() {
      this.removeMapInfoTrack()
      //  地图Source初始化
      //  点Source
      if (!this.map.getSource(this.track.layers.POINT_SOURCE)) {
        this.map.addSource(this.track.layers.POINT_SOURCE, {
          type: 'geojson',
          data: null
        })
      }
      //  线Source
      if (!this.map.getSource(this.track.layers.LINE_SOURCE)) {
        this.map.addSource(this.track.layers.LINE_SOURCE, {
          type: 'geojson',
          data: null
        })
      }

      //  Layer初始化
      //  点Layer
      if (!this.map.getLayer(this.track.layers.POINT_LAYER)) {
        this.map.addLayer({
          id: this.track.layers.POINT_LAYER,
          type: 'circle',
          source: this.track.layers.POINT_SOURCE,
          paint: {
            'circle-radius': { type: 'identity', property: 'DSize' },
            'circle-color': { type: 'identity', property: 'DColor' },
            'circle-opacity': { type: 'identity', property: 'DTranspatecy' }
          }
        })
      }

      //  线Dash的Layer
      if (!this.map.getLayer(this.track.layers.LINE_DASHED_LAYER)) {
        this.map.addLayer({
          id: this.track.layers.LINE_DASHED_LAYER,
          type: 'line',
          source: this.track.layers.LINE_SOURCE,
          layout: {
            'line-join': 'round',
            'line-cap': 'round'
          },
          paint: {
            'line-color': { type: 'identity', property: 'LColor' },
            'line-width': { type: 'identity', property: 'LWight' },
            'line-opacity': { type: 'identity', property: 'Transpatecy' },
            'line-dasharray': [2, 2]
          }
        })
      }

      //  线Layer
      if (!this.map.getLayer(this.track.layers.LINE_LAYER)) {
        this.map.addLayer({
          id: this.track.layers.LINE_LAYER,
          type: 'line',
          source: this.track.layers.LINE_SOURCE,
          layout: {
            'line-join': 'round',
            'line-cap': 'round'
          },
          paint: {
            'line-color': { type: 'identity', property: 'LColor' },
            'line-width': { type: 'identity', property: 'LWight' },
            'line-opacity': { type: 'identity', property: 'Transpatecy' }
          }
        })
      }

      // 支持实线及点画线
      this.map.setFilter(this.track.layers.LINE_LAYER, ['==', 'LStyle', 'solid'])
      this.map.setFilter(this.track.layers.LINE_DASHED_LAYER, ['==', 'LStyle', 'dash'])
    },
    //  删除轨迹回放地图信息
    removeMapInfoTrack() {
      if (this.popup != null) this.popup.remove()
      if (this.marker != null) this.marker.remove()
      if (this.map.getSource(this.track.layers.POINT_SOURCE)) {
        this.map.removeSource(this.track.layers.POINT_SOURCE)
      } if (this.map.getSource(this.track.layers.LINE_SOURCE)) {
        this.map.removeSource(this.track.layers.LINE_SOURCE)
      } if (this.map.getLayer(this.track.layers.POINT_LAYER)) {
        this.map.removeLayer(this.track.layers.POINT_LAYER)
      } if (this.map.getLayer(this.track.layers.LINE_DASHED_LAYER)) {
        this.map.removeLayer(this.track.layers.LINE_DASHED_LAYER)
      } if (this.map.getLayer(this.track.layers.LINE_LAYER)) {
        this.map.removeLayer(this.track.layers.LINE_LAYER)
      }
    },
    //  获取当前点地址
    getLocationAddress(feature) {
      if (feature.properties.address) {
        return Promise.resolve(feature.properties.address)
      }

      const p = this.reverseGeoCoding(feature)
      p.then((a) => {
        feature.properties.address = a
      })
      return p
    },
    //  地址编码转换
    reverseGeoCoding(feature) {
      // 当前点道路
      const roads = this.getRoadsAt(feature)
      return new Promise((resolve, reject) => {
        minemap.service.queryReverseGeoCodingResult(
          feature.geometry.coordinates.join(),
          2,
          100,
          100,
          1,
          function(error, results) {
            if (error) {
              // reject(error);
              resolve('')
              return
            }
            const { province, city, dist, roadname } = results.data
            let address = `${province}${city}${dist}${roadname || ''}`

            if (!roadname && roads.length) {
              address += roads[0].properties.name_zh || ''
            }
            resolve(address)
          }
        )
      })
    },
    //  从道路图层获取道路信息
    getRoadsAt(location) {
      const pt = this.map.project(location.geometry.coordinates)
      var features = this.map.queryRenderedFeatures(pt, { layers: ['positionLayer'] })
      return features
    },
    //  地图变化
    changeMap() {
      this.map.setStyle('http://mapsjgt.sz-its.cn/service/solu/style/id/4662')
    },
    //  切换地图状态
    changeStatus(status) {
      this.mapStatus = status
    },
    //  轨迹分段
    calcLinesLength(plocations) {
      var lines = {
        type: 'FeatureCollection',
        features: [this.getStartLine(plocations.features[0])]
      }
      lines = plocations.features.reduce((a, e, i) => {
        // last line
        const l = a.features.slice(-1)[0]
        l.geometry.coordinates.push(e.geometry.coordinates)
        if (!l.properties.points) {
          l.properties.points = []
        }
        l.properties.points.push(i)

        if (
          e.properties.Status != l.properties.Status &&
      i < plocations.features.length - 1
        ) {
          const nl = this.getStartLine(e)
          nl.geometry.coordinates.push(e.geometry.coordinates)
          nl.properties.points = [i]
          a.features.push(nl)
        }

        return a
      }, lines)

      const totalLength = turf.length(lines)
      lines.features.forEach((e) => {
        const sectLength = turf.length(e)
        e.properties.points.forEach((i, idx, a) => {
          const p = plocations.features[i].properties
          p.totalLength = totalLength
          p.sectLength = sectLength
        })
      })

      return lines
    },
    //  获取线路开始
    getStartLine(e) {
      return {
        type: 'Feature',
        geometry: {
          type: 'LineString',
          coordinates: []
        },
        properties: Object.assign({}, e.properties)
      }
    },
    //  播放事件
    play() {
      var vm = this
      this.track.consts.playStatus = this.track.consts.PLAYING
      //  暂时去除的--已解决
      //  playbackControl.updateState()
      //  暂时去除的--已解决
      //  playbackControl.setLabel(trackData.originalData.PData.Label)
      vm.updateState(this.track.trackData.originalData.HisLocation.PLocation[0], this.track.consts.playStatus)
      this.marker.addTo(this.map)

      var lineSource = this.map.getSource(this.track.layers.LINE_SOURCE)
      var pointSource = this.map.getSource(this.track.layers.POINT_SOURCE)
      lineSource.setData(this.track.trackData.lines)

      let t1 = Date.now()
      const interval = 1000 / vm.track.consts.FRAME_RATE

      function animate() {
        if (vm.track.consts.playStatus != vm.track.consts.PLAYING) {
          return
        }
        const now = Date.now()
        const d = now - t1
        if (d > interval) {
          t1 = now - (d % interval)

          const n = vm.track.playStep.next()
          if (n.value) {
          // lineSource.setData(n.value.line);
            pointSource.setData(n.value.point)

            const lastLine = n.value.line.features.slice(-1)[0]
            vm.marker.setLngLat(lastLine.geometry.coordinates.slice(-1)[0])

            const end = lastLine.geometry.coordinates
              .slice(-2)
              .map((e) => turf.point(e))
            let d = turf.bearing(end[0], end[1]) - 90
            // marker._inner.style.transform = `rotate(${d}deg)translateY(-20px)scale(0.8)`;
            const sx = d < -90 ? -0.8 : 0.8
            d = sx < 0 ? d + 180 : d
            vm.marker._inner.style.transform = `rotate(${d}deg)scaleX(${sx})scaleY(0.8)`

            vm.checkAutoTrack()

            requestAnimationFrame(animate)
          } else {
            vm.track.consts.playStatus = vm.track.consts.STOPPED
            //  暂时去除的
            //  playbackControl.updateState()
          }
        } else {
          requestAnimationFrame(animate)
        }
      }
      animate()
    },
    //  暂停
    pause() {
      this.track.consts.playStatus = this.track.consts.PAUSED
    },
    //  停止
    stop() {
      this.track.consts.playStatus = this.track.consts.STOPPED
      this.clear()
      this.track.playStep = this.step(this.track.trackData)
      //  设置点位回到起点
      //  this.track.trackData.plocations.features[0].properties.index = 0
      //  this.updateCurLocation(this.track.trackData.plocations.features[0], 0)
      //  cur.properties.index
    },
    //  清空轨迹回放层
    clear() {
      if (this.popup) {
        this.popup.remove()
      }
      if (this.marker) {
        this.marker.remove()
      }

      var lineSource = this.map.getSource(this.track.layers.LINE_SOURCE)
      var pointSource = this.map.getSource(this.track.layers.POINT_SOURCE)
      var trackpointSource = this.map.getSource(this.track.layers.TRACK_POSITION_SOURCE)
      if (lineSource) {
        lineSource.setData({
          type: 'FeatureCollection',
          features: []
        })
      }
      if (pointSource) {
        pointSource.setData({
          type: 'FeatureCollection',
          features: []
        })
      }
      if (trackpointSource) {
        trackpointSource.setData({
          type: 'FeatureCollection',
          features: []
        })
      }
    },
    //  速度变化
    speed(value) {
      this.track.speedRate = Number.parseInt(value)
    },
    //  地图跟踪
    checkAutoTrack() {
      if (!this.track.options.autoTrack) {
        return
      }
      const { lng, lat } = this.marker.getLngLat()
      const bounds = this.map.getBounds()
      if (
        lng < bounds.getWest() ||
        lng > bounds.getEast() ||
        lat < bounds.getSouth() ||
        lat > bounds.getNorth()
      ) {
        this.map.panTo([lng, lat])
      }
    },
    //  更新当前点
    updateCurLocation(cur, index = -1) {
      this.marker._inner.className = iconManager.getIconClass(cur.properties.Icon)
      //  暂时去除的
      // playbackControl.update(cur)
      if (index === -1) {
        this.handleEvent(
          'HisLocationPoint',
          this.track.trackData.originalData.HisLocation.PLocation[cur.properties.index]
        )
      } else {
        this.handleEvent(
          'HisLocationPoint',
          this.track.trackData.originalData.HisLocation.PLocation[0]
        )
      }
    },
    //  回调处理
    handleEvent(evt, arg) {
      if (this.track.trackData.originalData.PData.CallBack == 1) {
        const f = this.track.options.callback[evt]
        if (f) {
          f(arg)
        }
      }
    },
    //  步进控制
    * step(data) {
      var vm = this
      var len = 0
      var { plocations, lsymbols } = data
      var lines = {
        type: 'FeatureCollection',
        features: [vm.getStartLine(plocations.features[0])]
      }

      for (let i = 0; i < plocations.features.length; i++) {
        const cur = plocations.features[i]
        vm.updateCurLocation(cur)
        vm.updateState(cur.properties)
        // end point
        if (i == plocations.features.length - 1) {
          vm.updateState(vm.track.trackData.originalData.HisLocation.PLocation[0], vm.track.consts.playStatus)
          vm.pause()
          vm.$emit('updateState', {}, vm.track.consts.playStatus)
          yield {
            point: plocations,
            line: lines
          }
          vm.track.playStep = vm.step(data)
        }
        // 当前段
        const pts = plocations.features.slice(0, i + 2)
        // 轨迹点
        const point = {
          type: 'FeatureCollection',
          features: pts.slice(0, i + 1)
        }
        // 轨迹线
        let last = lines.features.slice(-1)[0]
        if (last.properties.Status != cur.properties.Status) {
          lines.features.push(vm.getStartLine(cur))
          last = lines.features.slice(-1)[0]
        }
        var lastCoords = [...last.geometry.coordinates]

        const tempLine = turf.lineString(
          pts.slice(-2).map((e) => e.geometry.coordinates)
        )

        const ld = turf.length(tempLine)
        let tempLen = 0
        const nn = 0
        while (tempLen + vm.track.speedRate * 0.005 < ld) {
          tempLen += vm.track.speedRate * 0.005
          const l = turf.lineSliceAlong(tempLine, 0, tempLen)
          last.geometry.coordinates = [...lastCoords, ...l.geometry.coordinates]
          yield {
            point: point,
            line: lines
          }
        }
        last.geometry.coordinates = [
          ...lastCoords,
          ...tempLine.geometry.coordinates
        ]
        yield {
          point: point,
          line: lines
        }
      }
    },
    //  地图定位
    setPosition(car, listCar = []) {
      //  判断是否为空
      if (car == null) {
        car = {
          DGUID: '',
          LUTC: '',
          LLON: '',
          LLAT: '',
          LSpeed: '',
          LDir: '',
          AlarmStatus: '',
          BusNo: '',
          DBusCard: ''
        }
      }
      //  地图状态为定位
      if (this.mapStatus === 'position') {
        const trans = CoordinatesTransform.wgs84togcj02(car.LLON, car.LLAT)
        const positionData = {
          Guid: car.DGUID,
          Label: car.DBusCard + '(' + car.LLAT + ' ' + car.LSpeed + ')', //  苏E-5F652(31.282865524292 速度:10)
          LON: trans[0],
          LAT: trans[1],
          Time: car.LUTC,
          Speed: car.LSpeed,
          Direction: car.LDir,
          Status: car.AlarmStatus,
          Icon: 'czc_1000',
          BusNo: car.BusNo,
          DBusCard: car.DBusCard,
          Remark: ''
        }

        const jsonData = {
          'type': 'FeatureCollection',
          'features': []
        }

        if (listCar.length <= 0) {
          jsonData.features.push({
            'type': 'Feature',
            'geometry': {
              'type': 'Point',
              'coordinates': [car.LLON, car.LLAT]
            },
            'properties': {
              'title': car.BusNo,
              'kind': 'other',
              'Label': car.Label,
              'Speed': car.LSpeed,
              'BusNo': car.BusNo,
              'DBusCard': car.DBusCard,
              'Remark': car.Remark,
              'Time': car.LUTC
            }
          })
        }

        listCar.map(item => {
          if (item.DGUID === car.DGUID) {
            jsonData.features.push({
              'type': 'Feature',
              'geometry': {
                'type': 'Point',
                'coordinates': [item.LLON, item.LLAT]
              },
              'properties': {
                'title': item.BusNo,
                'kind': 'click',
                'Label': item.Label,
                'Speed': item.LSpeed,
                'BusNo': item.BusNo,
                'DBusCard': item.DBusCard,
                'Remark': item.Remark,
                'Time': item.LUTC
              }
            })
          } else {
            jsonData.features.push({
              'type': 'Feature',
              'geometry': {
                'type': 'Point',
                'coordinates': [item.LLON, item.LLAT]
              },
              'properties': {
                'title': item.BusNo,
                'kind': 'other',
                'Label': item.Label,
                'Speed': item.LSpeed,
                'BusNo': item.BusNo,
                'DBusCard': item.DBusCard,
                'Remark': item.Remark,
                'Time': item.LUTC
              }
            })
          }
        })

        this.addMapInfoPosition()
        console.log('Layer.SetData', jsonData)
        this.setData(this.track.layers.POINT_SOURCE, jsonData, car.DGUID.length <= 0 ? listCar[0].LLON : positionData.LON, car.DGUID.length <= 0 ? listCar[0].LLAT : positionData.LAT)
      } else {
        //  地图状态为轨迹回放
        const trans = CoordinatesTransform.wgs84togcj02(car.LLON, car.LLAT)
        const positionData = {
          Guid: car.DGUID,
          Label: car.DBusCard + '(' + car.LLAT + ' ' + car.LSpeed + ')', //  苏E-5F652(31.282865524292 速度:10)
          LON: trans[0],
          LAT: trans[1],
          Time: car.LUTC,
          Speed: car.LSpeed,
          Direction: car.LDir,
          Status: car.AlarmStatus,
          Icon: 'czc_1000',
          BusNo: car.BusNo,
          DBusCard: car.DBusCard,
          Remark: ''
        }

        const jsonData = {
          'type': 'FeatureCollection',
          'features': []
        }
        jsonData.features.push({
          'type': 'Feature',
          'geometry': {
            'type': 'Point',
            'coordinates': [positionData.LON, positionData.LAT]
          },
          'properties': {
            'title': positionData.BusNo,
            'kind': 'click',
            'Label': positionData.Label,
            'Speed': positionData.Speed,
            'BusNo': positionData.BusNo,
            'DBusCard': positionData.DBusCard,
            'Remark': positionData.Remark,
            'Time': positionData.Time
          }
        })

        //  Source
        if (!this.map.getSource(this.track.layers.TRACK_POSITION_SOURCE)) {
          this.map.addSource(this.track.layers.TRACK_POSITION_SOURCE, {
            'type': 'geojson',
            'data': null
          })
        }
        //  Layer
        if (!this.map.getLayer(this.track.layers.TRACK_POSITION_LAYER)) {
          this.map.addLayer({
            'id': this.track.layers.TRACK_POSITION_LAYER,
            'type': 'circle',
            'source': this.track.layers.TRACK_POSITION_SOURCE,
            'layout': {
              'visibility': 'visible'
            },
            'paint': {
              'circle-radius': 10,
              'circle-color': {
                'type': 'categorical',
                'property': 'kind',
                'stops': [['click', 'red'], ['other', 'green']],
                'default': 'black'
              },
              'circle-opacity': 0.8
            },
            'minzoom': 7,
            'maxzoom': 17.5
          })
        }

        this.setData(this.track.layers.TRACK_POSITION_SOURCE, jsonData, positionData.LON, positionData.LAT)
      }
    },
    //  设置轨迹回放数据
    setTrackData(data) {
      var vm = this
      vm.addMapInfoTrack()
      data.HisLocation.PLocation.map(item => {
        const gcjloc2 = CoordinatesTransform.wgs84togcj02(item.LON, item.LAT)
        item.LON = gcjloc2[0]
        item.LAT = gcjloc2[1]
      })

      //  初始化轨迹回放的属性
      if (!data.PData) {
        data.PData = vm.track.options.TrackPlay.PData
      }
      if (!data.LSymbol) {
        data.LSymbol = vm.track.options.TrackPlay.LSymbol
      }

      data.PData.Label = data.HisLocation.PLocation[0].BusNo

      var plocations = {
        type: 'FeatureCollection',
        features: data.HisLocation.PLocation.map((e, i) => {
          return {
            type: 'Feature',
            geometry: {
              type: 'Point',
              coordinates: [e.LON, e.LAT]
            },
            properties: Object.assign({ index: i }, e)
          }
        })
      }

      // 保存样式模板
      var lsymbols = new Map()

      // 缺省格式
      lsymbols.set(vm.track.consts.DEFAULT_TEMPLATE_ID, {
        TemplateId: vm.track.consts.DEFAULT_TEMPLATE_ID,
        LColor: '#FF0000',
        LStyle: 'solid',
        LWight: 3,
        Transpatecy: 0.5,
        DColor: '#00ff00',
        DSize: 6,
        DTranspatecy: 0.8
      })
      // 接口数据样式
      data.LSymbol.forEach((e) => {
        lsymbols.set(e.TemplateId, e)
      })

      // 设置各点template属性
      plocations.features.forEach((e) => {
        const tempId = e.properties.TemplateId || vm.track.consts.DEFAULT_TEMPLATE_ID
        e.properties = Object.assign(e.properties, lsymbols.get(tempId))
      })

      if (vm.track.consts.playStatus != vm.track.consts.STOPPED) {
        vm.stop()
      }
      // 计算全程及分段长度
      const lines = vm.calcLinesLength(plocations)
      // 保存所有数据
      vm.track.trackData = {
        plocations,
        lines,
        lsymbols,
        reverseGeoCoding: data.PData.Address == 1,
        startIcon: data.PData.StartIcon,
        originalData: JSON.parse(JSON.stringify(data))
      }

      // 加载音频
      const loadAudio = alarmPlay.loadAlarmFiles(plocations)

      Promise.all([loadAudio, vm.track.init]).then(function() {
        // 清除地图
        vm.clear()
        // 路线
        var lineFeature = {
          type: 'Feature',
          geometry: {
            type: 'LineString',
            coordinates: plocations.features.map((p) => p.geometry.coordinates)
          }
        }

        vm.marker.remove()
        // 显示轨迹
        var lineSource = vm.map.getSource(vm.track.layers.LINE_SOURCE)
        // var pointSource = map.getSource(GUID.POINT_SOURCE);
        lineSource.setData(lines)
        // pointSource.setData(plocations);

        // 设置地图范围
        const bbox = turf.bbox(lineFeature)
        const bounds = minemap.LngLatBounds.convert(bbox)
        vm.map.fitBounds(bounds, { padding: 50 })

        // 初始化
        vm.marker.setLngLat(plocations.features[0].geometry.coordinates)
        const icon = iconManager.getIconClass(
          vm.track.options.TrackPlay.PData.StartIcon || plocations.features[0].properties.Icon
        )
        vm.marker._inner.className = icon
        vm.marker.addTo(vm.map)

        //  暂时去除的
        // playbackControl.setLabel(data.PData.Label)
        //  暂时去除的
        // playbackControl.update(plocations.features[0])
        vm.updateState(vm.track.trackData.originalData.HisLocation.PLocation[0])

        vm.track.playStep = vm.step(vm.track.trackData)

        if (vm.track.options.autoPlay) {
          vm.play()
        }
      })
    },
    //  更新轨迹回放的状态
    updateState(obj, state) {
      alarmPlay.updateState(this.track.consts.playStatus)
      this.$emit('updateState', obj, state)
      // if (state === this.track.consts.PAUSED) {
      //   this.pause()
      // } else if (state === this.track.consts.STOPPED) {
      //   this.stop()
      // } else if (state === this.track.consts.PLAYING) {
      //   this.play()
      // } else {
      //   this.msgError('Cancel invalid')
      // }
    },
    //  获取地图状态
    getMapStatus() {
      return this.mapStatus
    }
  }
}
</script>

<style lang="scss" scoped>
#map {
  width: 100%;
  height: 100%;
  position: relative;
  z-index: 10;
}
.track-popup {
  text-align: left;
  /* background-color:red; */
  background-color: rgba(0, 0, 0, 0.8);
  color: #ffffff;
  font-size: 0.8rem;
  padding: 0.8rem;
  border-radius: 0.5rem;
  border: 1px solid var(--popup-border-color);
  box-shadow: 1px 1px 2px var(--popup-border-color);
}
</style>
