
<template>

  <div class="app-container" @click="init.contextmenu.menuVisible = false">
    <el-drawer
      title="车辆查询"
      :visible.sync="init.draw.search.visible"
      direction="rtl"
      size="300px"
      @click.self.prevent
    >
      <div style="padding:10px;height:100%;overflow:auto;">
        <el-form rel="queryForm" :model="query" label-width="80px">
          <el-row>
            <el-col :span="24">
              <el-form-item label="自  编  号 ">
                <el-input v-model="query.BusNo" placeholder="请输入自编号" style="width:180px;" />
              </el-form-item>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="24">
              <el-form-item label="车  牌  号">
                <el-input v-model="query.DBusCard" placeholder="请输入车牌号" style="width:180px;" />
              </el-form-item>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="24">
              <el-form-item label="线路番号">
                <el-select v-model="query.LName" placeholder="请选择线路" style="width:180px;">
                  <el-option key="all" label="全部" value="1,10,126,900" />
                  <el-option key="1" label="1" value="1" />
                  <el-option key="10" label="10" value="10" />
                  <el-option key="126" label="126" value="126" />
                  <el-option key="900" label="900" value="900" />
                </el-select>
              </el-form-item>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="24" style="text-align:right;">
              <el-button type="primary" size="mini" @click="clickSearch">搜&#8195;&#8195;索</el-button>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="24" style="text-align:right;padding:5px 0px;">
              <el-button type="primary" size="mini" @click="clickPosition">定&#8195;&#8195;位</el-button>
              <el-button type="primary" size="mini" @click="clickTrackplay">轨迹回放</el-button>
              <el-button type="primary" size="mini" @click="clickMessage">发送消息</el-button>
            </el-col>
          </el-row>
          <el-row>
            <el-col :span="24">
              <div style="position:relative;">
                <el-form-item label="车辆列表" style="margin-bottom:0px;position:relative;" />
                <el-tree
                  id="el-tree"
                  ref="tree"
                  :data="data.treeData"
                  show-checkbox
                  node-key="id"
                  :props="init.tree.defaultProps"
                  style="height:320px;overflow:auto;"
                  @node-click="nodeClick"
                  @node-contextmenu="rightClick"
                />
                <div v-show="init.contextmenu.menuVisible">
                  <el-menu
                    id="rightClickMenu"
                    ref="contextmenu"
                    background-color="#409eff"
                    text-color="#ffffff"
                    active-text-color="#ffffff"
                    style="width:100px;"
                    @select="handleRightClick"
                  >
                    <el-menu-item index="track" class="menuItem">
                      <span slot="title">轨迹回放</span>
                    </el-menu-item>
                    <el-menu-item index="position" class="menuItem">
                      <span slot="title">定    位</span>
                    </el-menu-item>
                    <el-menu-item index="message" class="menuItem">
                      <span slot="title">发送消息</span>
                    </el-menu-item>
                    <el-menu-item index="video" class="menuItem">
                      <span slot="title">视频查看</span>
                    </el-menu-item>
                  </el-menu>
                </div>
              </div>
            </el-col>
          </el-row>
        </el-form>
      </div>
    </el-drawer>

    <el-drawer
      title="位置数据"
      :visible.sync="init.draw.table.visible"
      direction="btt"
      size="300px"
    >
      <div style="padding:10px;">
        <el-table
          :ref="init.table.key"
          :key="init.table.key"
          v-loading="loading"
          :border="true"
          :data="data.tableData"
          style="width:100%;"
          :fit="true"
          height="200px"
          :stripe="true"
          :empty-text="init.table.emptytext"
        >
          <el-table-column type="index" label="序号" align="center" width="80" />
          <el-table-column width="150" label="车牌号" property="DBusCard" align="center" />
          <el-table-column width="150" label="自编号" property="BusNo" align="center" />
          <el-table-column width="200" label="定位时间" property="LUTC" align="center" />
          <el-table-column width="200" label="经度" property="LLON" align="center" />
          <el-table-column width="200" label="纬度" property="LLAT" align="center" />
          <el-table-column width="120" label="速度" property="LSpeed" align="center" />
          <el-table-column label="操作">
            <template slot-scope="scope">
              <el-button size="mini" type="primary" @click="getPosition(scope.row)">定位</el-button>
            </template>
          </el-table-column>
        </el-table>
      </div>
    </el-drawer>

    <el-container :style="{height:init.container.height+'px'}" style="padding:0px;position:relative;">
      <Map ref="map" @updateState="updateState" />
      <div ref="mapmenu" class="mapmenu">
        <el-button type="primary" class="btn" @click="btnDrawCar">车辆查询</el-button>
        <el-button type="primary" class="btn" @click="btnDrawTable">位置数据</el-button>
        <!-- <el-button type="primary" class="btn" @click="getPosition(undefined)">定位</el-button> -->
      </div>
      <div class="maptrack">
        <TrackControl ref="controlTrack" @play="play" @pause="pause" @stop="stop" @close="close" @speed="speed" />
      </div>
    </el-container>

    <TrackPlay :options="init.dialog" @callback="callback" />
    <SendMessage ref="message" :options="init.message" @callback="callback" />
    <VideoControl ref="video" :options="init.video" />
  </div>
</template>

<script>
import request from '@/utils/action.js'
import convert from '@/utils/convert.js'
import communicate from '@/utils/communicate.js'
import Map from './mineMap.vue'
import TrackPlay from './trackplay.vue'
import SendMessage from './sendmessage.vue'
import TrackControl from './TrackPlaybackControl.vue'
import VideoControl from './VideoControl.vue'

export default {
  name: 'List',
  components: { Map, TrackPlay, TrackControl, SendMessage, VideoControl },
  data: function() {
    return {
      nodeData: {},
      // 遮罩层
      loading: false,
      // 合计数量
      total: 0,
      //  第一次加载
      fisrtLoad: false,
      //  当前选中的车辆信息
      arrTempBusNo: [],
      //  查询绑定参数
      query: {
        CurrentPage: 0,
        PageSize: 99999,
        LName: null,
        BusNo: null,
        DBusCard: null
      },
      //  当前定位点击车辆
      clickCar: null,
      //    初始化参数
      init: {
        //  定时器
        timers: {
          //  定时获取选中车辆的点位数据
          timerGetPosition: {
            timer: null,
            second: 30000
          },
          //  定时进行轨迹回放
          timerTrackplay: {
            timer: null,
            second: 3000
          }
        },
        //  右键菜单
        contextmenu: {
          contextMenuTarget: null,
          menuVisible: false
        },
        //  选项卡
        tabs: {
          active: 'real'
        },
        //  查询条件初始化参数
        search: {
          corp: [],
          line: [],
          shift: []
        },
        table: {
          height: window.innerHeight - 308,
          key: 'table',
          emptytext: '暂无数据',
          datatype: 'AttendanceDataReport',
          listnode: 'AttendanceData'
        },
        //  轨迹回放窗体
        dialog: {
          width: '50%',
          showDialog: false,
          title: '',
          type: 'detail',
          BusNo: null,
          CarNo: null
        },
        //  消息发送窗体
        message: {
          width: '350px',
          showDialog: false,
          title: '',
          type: 'detail',
          ReceiverGUID: null,
          DBusCard: null
        },
        //  消息发送窗体
        video: {
          width: '800px',
          showDialog: false,
          title: '',
          type: 'detail',
          BusNo: null,
          DBusCard: null
        },
        container: {
          height: window.innerHeight - 135
        },
        tree: {
          defaultProps: {
            children: 'children',
            label: 'label'
          }
        },
        draw: {
          table: {
            visible: false
          },
          search: {
            visible: false
          }
        }
      },
      //    表格数据
      data: {
        treeData: new Array(),
        tableData: []
      },
      pager: {
        page: 1,
        limit: 10
      }
    }
  },
  mounted: function() {
    if (!this.fisrtLoad) {
    //  初始化控件
      this.initControls()
      //  初始化定时器
      this.initTimers()
      this.fisrtLoad = true
    }
  },
  methods: {
    //  开启定位定时器
    startTimerPosition() {
      const vm = this
      this.init.timers.timerGetPosition.timer = setInterval(() => {
        const selectNodes = vm.$refs.tree ? vm.$refs.tree.getCheckedNodes() : []
        if (selectNodes.length <= 0) {
          return
        }
        const arrBusNo = []
        selectNodes.map(item => {
          if (item.obj) {
            arrBusNo.push(item.obj.BusNo)
            //  vm.arrTempBusNo.push(item)
          }
        })
        //  根据车联编码获取定位信息，显示到表格中
        communicate
          .TransformData(
            'LocationInfoList',
            {
              PageSize: 99999,
              CurrentPage: 0,
              CompGUID: '',
              BusNo: '' + arrBusNo.join(',') + ''
            },
            'cardata',
            'cardata',
            'cardata'
          )
          .then(data => {
            vm.data.tableData = convert.object2array(data.LocationInfo)
            vm.$refs.map.setPosition(vm.clickCar, vm.data.tableData)
          }).catch(error => {
            console.log(error)
            this.msgError(this.errorMessage['Commmons']['1'])
          })
      }, this.init.timers.timerGetPosition.second)
    },
    //  关闭定位定时器
    stopTimerPosition() {
      clearInterval(this.init.timers.timerGetPosition.timer)
    },
    //  右击菜单--车辆定位
    btnDrawCar() {
      this.init.draw.search.visible = true
    },
    //  右击菜单--数据查询
    btnDrawTable() {
      this.init.draw.table.visible = true
    },
    //  初始化控件
    initControls() {
      var vm = this
      //  设置数据表表格为空
      this.data.tableData = []
      //  进入初始化方法
      this.data.treeData = new Array()
      //  获取线路信息，并初始化树形结构线路节点
      this.data.treeData.push({
        id: '1',
        label: '1',
        children: []
      })
      this.data.treeData.push({
        id: '10',
        label: '10',
        children: []
      })
      this.data.treeData.push({
        id: '126',
        label: '126',
        children: []
      })
      this.data.treeData.push({
        id: '900',
        label: '900',
        children: []
      })
      if (!this.query.LName) {
        this.query.LName = '1,10,126,900'
      }
      if (!this.query.BusNo) {
        this.$delete(this.query, 'BusNo')
      }
      if (!this.query.DBusCard) {
        this.$delete(this.query, 'DBusCard')
      }
      //  根据线路信息获取所有车辆信息
      communicate
        .TransformData('BusParkingSiteList', this.query)
        .then(data => {
          //  所有线路车辆信息
          var carInfos = convert.object2array(data.BusParkingSiteInfo)
          //  存储车辆编码数组集合（去重）
          const arrBusNo = []
          if (carInfos.length > 0) {
            //  初始化线路车辆树形结构信息
            this.data.treeData.map(item => {
              const cars = carInfos.filter(car => car.LName.toString() == item.id.toString())
              if (cars && cars.length > 0) {
                cars.map(car => {
                  if (arrBusNo.indexOf(car.BusNo) < 0) {
                    //  生成车牌数组字符串获取
                    arrBusNo.push(car.BusNo)
                    //  树形结构添加子节点
                    item.children.push({
                      id: car.GUID,
                      label: car.BusNo + ' (' + car.DBusCard + ')',
                      obj: car
                    })
                  }
                })
              }
            })
          }
          //  线路节点显示车辆数量
          this.data.treeData.map(item => {
            item.label += '(' + item.children.length + ')'
          })
        }).catch(error => {
          console.log(error)
          this.msgError(this.errorMessage['Commmons']['1'])
        })
    },
    //  初始化定时器
    initTimers() {
      //  初始化定时获取节点表
      //  this.startTimerPosition()
    },
    //  定位信息
    getPosition(row) {
      if (!row) {
        row = { 'DGUID': '10512101087', 'DBusCard': '苏E-5F555', 'BusNo': '101087', 'LUTC': '2020/2/21 11:10:48', 'LLON': '120.590408325195', 'LLAT': '31.2938365936279', 'LHigh': '13', 'LSpeed': '18', 'LDir': '193', 'TotalMiles': '0', 'SatelliteNum': '25', 'LVFlag': '0', 'LVDesc': '未知', 'CreateTime': '2020/2/21 11:10:49', 'TimeDifference': '1', 'AlarmStatus': '0' }
        //  this.$refs.mapmenu.style.display = 'none'
      }
      this.clickCar = row
      this.$refs.map.setPosition(row, this.data.tableData)
    },
    //  轨迹回放窗体回调函数
    callback(type, queryform) {
      const vm = this
      if (type === 'track') {
        const myload = vm.inloading('轨迹查询中，请稍后......')
        //  清空定时器
        vm.stopTimerPosition()
        //  获取轨迹回放数据
        communicate.TransformData('HisLocationInfoList', {
          SortMethod: 'LUTC',
          OrderMethod: 'asc',
          PageSize: '10000',
          CurrentPage: 0,
          BusNo: queryform.BusNo,
          StartTime: this.parseTime(queryform.StartTime, '{y}-{m}-{d} {h}:{i}') + ':00',
          EndTime: this.parseTime(queryform.EndTime, '{y}-{m}-{d} {h}:{i}') + ':59'
        }, 'cardata', 'cardata', 'cardata').then(data => {
          vm.data.tableData = convert.object2array(data.LocationInfo)
          vm.init.tabs.active = 'history'
          var list = vm.data.tableData.map(item => {
            // const gcjloc2 = CoordinatesTransform.wgs84togcj02(item.LLON, item.LLAT)
            return {
              Guid: item.DGUID,
              Label: item.DBusCard + '(' + item.LLAT + ' ' + item.LSpeed + ')', //  苏E-5F652(31.282865524292 速度:10)
              LON: item.LLON,
              LAT: item.LLAT,
              Time: item.LUTC,
              Speed: item.LSpeed,
              Direction: item.LDir,
              Status: item.AlarmStatus,
              Icon: 'gjc_1500',
              Remark: '',
              BusNo: item.BusNo,
              DBusCard: item.DBusCard
            }
          })
          vm.unloading(myload)

          if (list.length <= 0) {
            vm.msgError('无轨迹数据')
            return
          }

          vm.$refs.controlTrack.show()
          vm.$refs.map.changeStatus('track')

          var obj = {
            HisLocation: {
              LName: 'Address',
              PLocation: list
            }
          }
          vm.$refs.map.setTrackData(obj)
          // vm.$refs.map.setData_DisplayControlBar({
          //   DisplayControlBar: {
          //     Display: 1
          //   }
          // })
        }).catch(error => {
          console.log(error)
          vm.unloading(myload)
          this.msgError(this.errorMessage['Commmons']['1'])
        })
      } else if (type === 'message') {
        this.init.message.showDialog = false
      }
      this.init.dialog.showDialog = false
      this.init.draw.search.visible = false
    },
    //  消息窗体回调函数
    callMessage(type, queryform) {
      const vm = this
      if (type == 'track') {
        const myload = vm.inloading('轨迹查询中，请稍后......')
        //  清空定时器
        vm.stopTimerPosition()
        //  获取轨迹回放数据
        communicate.TransformData('HisLocationInfoList', {
          SortMethod: 'LUTC',
          OrderMethod: 'asc',
          PageSize: '10000',
          CurrentPage: 0,
          BusNo: queryform.BusNo,
          StartTime: this.parseTime(queryform.StartTime, '{y}-{m}-{d} {h}:{i}') + ':00',
          EndTime: this.parseTime(queryform.EndTime, '{y}-{m}-{d} {h}:{i}') + ':59'
        }, 'cardata', 'cardata', 'cardata').then(data => {
          vm.data.tableData = convert.object2array(data.LocationInfo)
          vm.init.tabs.active = 'history'
          var list = vm.data.tableData.map(item => {
            // const gcjloc2 = CoordinatesTransform.wgs84togcj02(item.LLON, item.LLAT)
            return {
              Guid: item.DGUID,
              Label: item.DBusCard + '(' + item.LLAT + ' ' + item.LSpeed + ')', //  苏E-5F652(31.282865524292 速度:10)
              LON: item.LLON,
              LAT: item.LLAT,
              Time: item.LUTC,
              Speed: item.LSpeed,
              Direction: item.LDir,
              Status: item.AlarmStatus,
              Icon: 'gjc_1500',
              Remark: '',
              BusNo: item.BusNo,
              DBusCard: item.DBusCard
            }
          })
          vm.unloading(myload)

          if (list.length <= 0) {
            vm.msgError('无轨迹数据')
            return
          }

          vm.$refs.controlTrack.show()

          var obj = {
            HisLocation: {
              LName: 'Address',
              PLocation: list
            }
          }
          vm.$refs.map.setTrackData(obj)
          // vm.$refs.map.setData_DisplayControlBar({
          //   DisplayControlBar: {
          //     Display: 1
          //   }
          // })
        })
      }
      this.init.dialog.showDialog = false
      this.init.draw.search.visible = false
    },
    //  树形菜单点击事件
    nodeClick(data) {
      this.init.contextmenu.menuVisible = false
      const vm = this
      return
      const selectNodes = vm.$refs.tree ? vm.$refs.tree.getCheckedNodes() : []
      if (selectNodes.length <= 0) {
        return
      }
      const arrBusNo = []
      selectNodes.map(item => {
        if (item.obj) {
          arrBusNo.push(item.obj.BusNo)
        }
      })
      //  根据车联编码获取定位信息，显示到表格中
      communicate
        .TransformData(
          'LocationInfoList',
          {
            PageSize: 99999,
            CurrentPage: 0,
            CompGUID: '',
            BusNo: '' + arrBusNo.join(',') + ''
          },
          'cardata',
          'cardata',
          'cardata'
        )
        .then(data => {
          vm.data.tableData = convert.object2array(data.LocationInfo)
        }).catch(error => {
          console.log(error)
          this.msgError(this.errorMessage['Commmons']['1'])
        })
    },
    //  树形菜单check选中事件
    nodeCheck(checkNode, treeCheckStatus) {
      const vm = this
      var { checkedNodes } = treeCheckStatus
      const selectNodes = !checkedNodes ? [] : checkedNodes
      if (selectNodes.length <= 0) {
        return
      }
      const arrBusNo = []
      selectNodes.map(item => {
        if (item.obj) {
          arrBusNo.push(item.obj.BusNo)
          //  vm.arrTempBusNo.push(item)
        }
      })
      //  根据车联编码获取定位信息，显示到表格中
      communicate
        .TransformData(
          'LocationInfoList',
          {
            PageSize: 99999,
            CurrentPage: 0,
            CompGUID: '',
            BusNo: '' + arrBusNo.join(',') + ''
          },
          'cardata',
          'cardata',
          'cardata'
        )
        .then(data => {
          vm.data.tableData = convert.object2array(data.LocationInfo)
        }).catch(error => {
          console.log(error)
          this.msgError(this.errorMessage['Commmons']['1'])
        })
    },
    //  按钮--车辆查询
    clickSearch() {
      this.initControls()
    },
    //  按钮--轨迹回放
    clickTrackplay() {
      var vm = this
      const selectNodes = vm.$refs.tree ? vm.$refs.tree.getCheckedNodes() : []
      var cars = selectNodes.filter(item => item.obj)
      if (cars.length !== 1) {
        vm.msgError('有且只能选择一辆车')
        return
      }
      this.init.dialog.BusNo = cars[0].obj.BusNo
      this.init.dialog.CarNo = cars[0].obj.DBusCard
      this.init.dialog.showDialog = true
      this.stopTimerPosition()
    },
    //  按钮--定位
    clickPosition() {
      var mapstate = this.$refs.map.getMapStatus()
      if (mapstate === 'track') {
        this.msgError('当前正在轨迹回放，无法开启实时定位')
        return
      }
      const vm = this
      //  获取数据
      const selectNodes = vm.$refs.tree ? vm.$refs.tree.getCheckedNodes() : []
      if (selectNodes.length <= 0) {
        this.msgError('请至少选择一辆车进行定位')
        return
      }
      const arrBusNo = []
      selectNodes.map(item => {
        if (item.obj) {
          arrBusNo.push(item.obj.BusNo)
          //  vm.arrTempBusNo.push(item)
        }
      })
      debugger
      //  根据车联编码获取定位信息，显示到表格中
      communicate
        .TransformData(
          'LocationInfoList',
          {
            PageSize: 99999,
            CurrentPage: 0,
            CompGUID: '',
            BusNo: '' + arrBusNo.join(',') + ''
          },
          'cardata',
          'cardata',
          'cardata'
        )
        .then(data => {
          vm.data.tableData = convert.object2array(data.LocationInfo)
          if (vm.data.tableData.length > 0) {
            vm.$refs.map.setPosition(data.LocationInfo[0], vm.data.tableData)
          }
          vm.startTimerPosition()
        }).catch(error => {
          console.log(error)
          this.msgError(this.errorMessage['Commmons']['1'])
        })
    },
    //  按钮--发送消息
    clickMessage() {
      var vm = this
      //  获取数据
      const selectNodes = vm.$refs.tree ? vm.$refs.tree.getCheckedNodes() : []
      if (selectNodes.length <= 0) {
        vm.msgError('请至少选择一辆车进行定位')
        return
      }

      const arrBusNo = []
      const arrDGuid = []
      selectNodes.map(item => {
        if (item.obj) {
          arrBusNo.push(item.obj.BusNo)
          arrDGuid.push(item.obj.DGUID)
          //  vm.arrTempBusNo.push(item)
        }
      })

      vm.init.message.BusNo = arrBusNo
      vm.init.message.ReceiverGUID = arrDGuid
      vm.init.message.showDialog = true
    },
    //  右击菜单--轨迹回放
    rightClickTrackplay() {
      this.init.dialog.BusNo = this.nodeData.obj.BusNo
      this.init.dialog.CarNo = this.nodeData.obj.DBusCard
      this.init.dialog.showDialog = true
      this.stopTimerPosition()
    },
    //  右击菜单--定位
    rightClickPosition() {
      var vm = this
      communicate
        .TransformData(
          'LocationInfoList',
          {
            PageSize: 99999,
            CurrentPage: 0,
            CompGUID: '',
            BusNo: '' + vm.nodeData.obj.BusNo
          },
          'cardata',
          'cardata',
          'cardata'
        )
        .then(data => {
          if (data.LocationInfo) {
            vm.$refs.map.setPosition(data.LocationInfo, [])
          } else {
            vm.msgError('车辆[' + vm.nodeData.obj.BusNo + ']无实时数据')
          }
        }).catch(error => {
          console.log(error)
          this.msgError(this.errorMessage['Commmons']['1'])
        })
    },
    //  右击菜单--发送消息
    rightClickMessage() {
      this.init.message.BusNo = this.helpers.convert.object2array(this.nodeData.obj.BusNo)
      this.init.message.ReceiverGUID = this.helpers.convert.object2array(this.nodeData.obj.DGUID)
      this.init.message.showDialog = true
    },
    //  视频播放
    rightClickVideo() {
      this.init.video.BusNo = this.nodeData.obj.BusNo
      this.init.video.DBusCard = this.nodeData.obj.DBusCard
      this.init.video.showDialog = true
    },
    //  列format事件
    formatter: function(row, column, cellValue, index) {},
    //  获取数据
    getlist: function() {
      var vm = this
      var url = vm.init.table.url
      if (!url || url.length <= 0) {
        return
      }
      vm.query.pageIndex = vm.pager.index
      vm.query.pageSize = vm.pager.size
      request({
        url: vm.init.table.url,
        type: vm.init.table.type,
        data: vm.query,
        success: function(data) {
          if (data.success) {
            vm.data = data.data
            vm.pager.total = data.count
          }
        }
      })
    },
    //  查询按钮
    btnQuery: function() {
      this.getlist()
    },
    //  右键菜单点击
    handleRightClick(key) {
      if (key == 'track') {
        this.rightClickTrackplay()
      } else if (key == 'position') {
        this.rightClickPosition()
      } else if (key == 'message') {
        this.rightClickMessage()
      } else if (key == 'video') {
        this.rightClickVideo()
      } else {
        this.msgError('操作异常')
      }
      this.init.contextmenu.menuVisible = false
    },
    //  树形菜单右击事件
    rightClick(event, object, value, element) {
      if (object.children) {
        return
      }
      // const vm = this
      // document.addEventListener('click', (e) => {
      //   vm.init.contextmenu.menuVisible = false
      // })
      this.init.contextmenu.menuVisible = true
      const menu = document.querySelector('#rightClickMenu')
      /* 菜单定位基于鼠标点击位置 */
      menu.style.left = event.layerX + 20 + 'px'
      menu.style.top = event.layerY - 30 + 'px'
      // menu.style.left = event.layerX - event.offsetX + 'px'
      // menu.style.top = event.layerY + event.offsetY + 'px'
      menu.style.position = 'absolute' // 为新创建的DIV指定绝对定位
      menu.style.cursor = 'pointer'
      this.nodeData = object
    },
    //  轨迹回放开始
    play() {
      this.$refs.map.play()
    },
    //  轨迹回放暂停
    pause() {
      this.$refs.map.pause()
    },
    //  轨迹回放停止
    stop() {
      this.$refs.map.stop()
    },
    //  轨迹回放关闭
    close() {
      this.stop()
      this.data.tableData = []
      //  this.startTimerPosition()
      this.$refs.map.changeStatus('position')
    },
    //  变化速度
    speed(speed) {
      this.$refs.map.speed(speed)
    },
    //  更新状态
    updateState(obj, state) {
      this.$refs.controlTrack.updateState(obj, state)
    }
  }
}
</script>

<style scoped>
span{
  outline-style: none;
}
.area {
  margin: 5px auto;
}
.label {
  text-align: center;
  line-height: 36px;
  font-size: 14px;
}

.el-drawer__header span {
  outline:none;
}

html,
body {
  height: 100%;
  font-size: 14px;
}
#app {
  width: 800px;
  margin: 0 auto;
  font-family: "Microsoft Yahei", "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
  height: 100%;
}
/* 右键会选中文字，为了美观将它禁用 */
#el-tree {
  user-select: none;
}
.right-menu {
  position: fixed;
  background: #fff;
  border: solid 1px rgba(0, 0, 0, 0.2);
  border-radius: 3px;
  z-index: 999;
  display: none;
}
.right-menu a {
  width: 75px;
  height: 28px;
  line-height: 28px;
  text-align: center;
  display: block;
  color: #1a1a1a;
}
.right-menu a:hover {
  background: #eee;
  color: #fff;
}
.right-menu {
  border: 1px solid #eee;
  box-shadow: 0 0.5em 1em 0 rgba(0, 0, 0, 0.1);
  border-radius: 1px;
}
a {
  text-decoration: none;
}
.right-menu a {
  padding: 2px;
}
.right-menu a:hover {
  background: #42b983;
}
.mapmenu{
  position:absolute;
  right:20px;
  top:20px;
  z-index:10;
  background-color: rgba(0, 0, 0, 0.6);
  padding:5px 15px;
  border-radius: 3px;
}

.maptrack{
  position:absolute;
  right:16px;
  top:80px;
  z-index:10;
  border-radius: 3px;
}

.btn{
  background:transparent;
  border:none;
}
.btn:hover{
  background:rgba(0, 0, 0, 0.7);
}
.el-drawer.rtl{
    overflow: scroll;
}
.menuItem{
  height:36px;
  width:100px;
  line-height:36px;
  cursor:pointer;
}
</style>
