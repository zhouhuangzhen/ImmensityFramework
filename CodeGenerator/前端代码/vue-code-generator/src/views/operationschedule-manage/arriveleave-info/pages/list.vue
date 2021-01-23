<template>
  <div class="app-container">
    <!--  查询条件  -->
    <div class="area">
      <el-form ref="queryForm" :model="query" label-width="120px">
        <el-row>
          <el-col :span="8">
            <el-form-item label="线路编号">
              <el-select v-model="query.LName" style="width:210px;" @change="linechange">
                <el-option
                  v-for="item in init.search.lineNumber"
                  :key="item.key"
                  :label="item.label"
                  :value="item.value"
                />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="线路方向">
              <el-select v-model="query.LGUID" style="width:210px;">
                <el-option
                  v-for="item in init.search.lineDirect"
                  :key="item.key"
                  :label="item.label"
                  :value="item.value"
                />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="车牌号">
              <el-input v-model="query.DBusCard" placeholder="请输入车牌号" style="width:210px;" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="企业自编号">
              <el-input v-model="query.BusNo" placeholder="请输入企业自编号" style="width:210px;" />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="进站开始时间">
              <el-date-picker
                v-model="query.initStartTime"
                type="date"
                format="yyyy-MM-dd"
                value-format="yyyy-MM-dd"
                placeholder="请选择进站开始时间"
                style="width:210px;"
                :picker-options="pickerOptionsStart"
                @change="changeEnd"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="进站结束时间">
              <el-date-picker
                v-model="query.initEndTime"
                type="date"
                format="yyyy-MM-dd"
                value-format="yyyy-MM-dd"
                placeholder="请选择进站结束时间"
                style="width:210px;"
                :picker-options="pickerOptionsEnd"
                @change="changeStart"
              />
            </el-form-item>
          </el-col>
        </el-row>
      </el-form>
    </div>
    <!--  按钮区域  -->
    <div style="text-align:right;" class="area">
      <el-button type="primary" size="small" @click="btnQuery">
        <i class="el-icon-search" />
        查询
      </el-button>
      <el-button type="info" size="small" class="el-button--export" @click="btnExport">
        <i class="el-icon-download" />
        导出
      </el-button>
    </div>
    <!--  数据表格  -->
    <div class="area">
      <el-table
        :ref="init.table.key"
        :key="init.table.key"
        v-loading="loading"
        :border="true"
        :data="data"
        style="width:100%;"
        :fit="true"
        :height="init.table.height"
        :stripe="true"
        :empty-text="init.table.emptytext"
      >
        <el-table-column :label="$t('detail.index')" align="center" width="70">
          <template scope="scope"><span>{{ scope.$index+(query.CurrentPage) * query.PageSize + 1 }} </span></template>
        </el-table-column>
        <el-table-column width="180" label="车载设备标识" property="DGUID" align="center" />
        <el-table-column width="100" label="企业自编号" property="BusNo" align="center" />
        <el-table-column width="200" label="车牌号" property="DBusCard" align="center" />
        <el-table-column width="200" label="线路番号" property="LName" align="center" />
        <el-table-column width="180" label="线路方向" property="LDirection" align="center" />
        <el-table-column width="100" label="站序" property="SLNO" align="center" />
        <el-table-column width="200" label="站点名称" property="LSName" align="center" />
        <el-table-column width="150" label="进站时间" property="DInTime" align="center" />
        <el-table-column width="150" label="出站时间" property="DOutTime" align="center" />
        <el-table-column width="100" label="车辆方向" property="LDir" align="center" />
        <el-table-column width="100" label="数据来源" property="SDevice" align="center" />
        <el-table-column label="状态描述" property="LVDesc" align="center" />
      </el-table>

      <pagination
        v-show="total>0"
        :total="total"
        :page.sync="pager.page"
        :limit.sync="pager.limit"
        @pagination="getlist"
      />
    </div>
  </div>
</template>

<script>
import communicate from '@/utils/communicate.js'
import { baseinfo, userinfo } from '@/utils/infohelper.js'
import excelhelper from '@/utils/excelhelper.js'
import datehelper from '@/utils/datehelper.js'
import convert from '@/utils/convert.js'

export default {
  name: 'List',
  components: {},
  data: function() {
    return {
      pickerOptionsStart: {},
      pickerOptionsEnd: {},
      // 遮罩层
      loading: false,
      // 合计数量
      total: 0,
      // 时间区间
      Time: [],
      //  查询绑定参数
      query: {
        CurrentPage: 1,
        PageSize: 10,
        SortMethod: 'NInTime',
        OrderMethod: 'asc',
        initStartTime: null,
        initEndTime: null,
        StartTime: null,
        EndTime: null,
        LName: null,
        LGUID: null,
        DBusCard: null,
        BusNo: null
      },
      //    初始化参数
      init: {
        //  查询条件初始化参数
        search: {
          lineNumber: [],
          lineDirect: [
            {
              key: 'empty',
              value: '',
              label: '--请选择--'
            }
          ],
          lineInfos: []
        },
        table: {
          height: window.innerHeight - 366,
          key: 'table',
          emptytext: '暂无数据',
          datatype: 'HisBusPositionInfoList',
          listnode: 'PositionInfo'
        },
        dialog: {
          width: '50%',
          showDialog: false,
          title: '',
          type: 'detail',
          Id: null
        },
        sort: {
          SortMethod: 'SignTime',
          OrderMethod: 'desc'
        }
      },
      //    表格数据
      data: [],
      pager: {
        page: 1,
        limit: 10
      }
    }
  },
  created: function() {
    //  初始化线路下拉数据
    // communicate("TransformData", vm.init.table.datatype, vm.query).then(
    //     response => {
    //       console.log(response);
    //       vm.data = response[vm.init.table.listnode];
    //       vm.pager.total = 100;
    //     }
    //   );
  },
  mounted: function() {
    this.initData()
    //  初始化表格数据
    //  this.getlist()
  },
  methods: {
    changeStart() {
      this.pickerOptionsStart = Object.assign({}, this.pickerOptionsStart, {
        disabledDate: (time) => {
          if (!this.query.initEndTime) return false
          return time.getTime() > new Date(this.query.initEndTime).getTime()
        }
      })
    },
    changeEnd() {
      this.pickerOptionsEnd = Object.assign({}, this.pickerOptionsEnd, {
        disabledDate: (time) => {
          if (!this.query.initStartTime) return false
          return time.getTime() < new Date(this.query.initStartTime).getTime()
        }
      })
    },
    //
    initData() {
      this.query.initStartTime = this.helpers.date.fromToday(-1)
      this.query.initEndTime = this.helpers.date.fromToday(-1)
      //  初始化线路编号下拉框的数据
      this.init.search.lineNumber = []
      this.helpers.info.userinfo.getLinesNew().then(data => {
        const all = []
        debugger
        this.init.search.lineInfos = this.helpers.convert.object2array(data.List)
        this.init.search.lineInfos.map(item => {
          if (all.indexOf(item.LName) < 0) {
            all.push(item.LName)
            this.init.search.lineNumber.push({
              key: item.LName,
              value: item.LName,
              label: item.LName
            })
          }
        })
        this.init.search.lineNumber.unshift({
          key: 'all',
          value: "'" + all.join("','") + "'",
          label: '全部'
        })
        this.query.LName = "'" + all.join("','") + "'"
      })
    },
    //  线路下拉值变化事件
    linechange: function(changeValue) {
      var vm = this
      vm.init.search.lineDirect = [
        {
          key: 'empty',
          value: '',
          label: '--请选择--'
        }
      ]
      var lines = vm.init.search.lineInfos.filter(item => item.LName === changeValue)
      lines.map(item => {
        vm.init.search.lineDirect.push({
          label: item.LDirection,
          key: item.Guid,
          value: item.Guid
        })
      })
    },
    //  列format事件
    formatter: function(row, column, cellValue, index) {
      let value = null
      switch (column.label) {
        case '调度类型': {
          if (!cellValue) {
            value = '新增'
          } else {
            value = '变动'
          }
          break
        }
      }
      return value
    },
    //  获取数据
    getlist: function(pager) {
      var vm = this
      if (this.$refs[this.init.table.key]) this.$refs[this.init.table.key].bodyWrapper.scrollTop = 0
      vm.loading = true
      //  接口对象处理
      if (!vm.query.DBusCard) {
        vm.$delete(vm.query, 'DBusCard')
      }
      if (!vm.query.BusNo) {
        vm.$delete(vm.query, 'BusNo')
      }
      if (!vm.query.LGUID) {
        vm.$delete(vm.query, 'LGUID')
      }
      if (!vm.query.LName) {
        vm.$delete(vm.query, 'LName')
      }
      if (!vm.query.initStartTime) {
        vm.$delete(vm.query, 'StartTime')
      } else {
        vm.query.StartTime = vm.query.initStartTime + ' 00:00:00.000'
      }
      if (!vm.query.initEndTime) {
        vm.$delete(vm.query, 'EndTime')
      } else {
        vm.query.EndTime = vm.query.initEndTime + ' 23:59:59.999'
      }

      if (pager === undefined) {
        vm.query.CurrentPage = 0
      } else {
        vm.query.CurrentPage = pager.page - 1
        vm.query.PageSize = pager.limit
        vm.pager = pager
      }
      communicate.optAxios.withCredentials = false
      communicate
        .TransformData(vm.init.table.datatype, vm.query, 'cardata', 'cardata')
        .then(response => {
          console.log('response : ', response)
          vm.data = []
          if (response[vm.init.table.listnode]) {
            if (Array.isArray(response[vm.init.table.listnode])) {
              vm.data = response[vm.init.table.listnode]
            } else {
              vm.data.push(response[vm.init.table.listnode])
            }
            vm.total = response._Total
            vm.loading = false
          } else {
            vm.data = []
            vm.total = 0
            vm.loading = false
          }
        }).catch(error => {
          console.log(error)
          vm.loading = false
          this.msgError(this.errorMessage['Commmons']['1'])
        })
    },

    //  查询按钮
    btnQuery: function() {
      if (
        !this.query.initStartTime ||
        this.query.initStartTime.length <= 0 ||
        !this.query.initEndTime ||
        this.query.initEndTime.length <= 0) {
        this.msgError('开始时间和结束时间不能为空')
        return
      }
      this.pager.page = 1
      this.getlist()
    },
    //  导出按钮
    btnExport: function() {
      var vm = this
      //  接口对象处理
      if (!vm.query.DBusCard) {
        vm.$delete(vm.query, 'DBusCard')
      }
      if (!vm.query.BusNo) {
        vm.$delete(vm.query, 'BusNo')
      }
      if (!vm.query.LGUID) {
        vm.$delete(vm.query, 'LGUID')
      }
      if (!vm.query.LName) {
        vm.$delete(vm.query, 'LName')
      }
      if (!vm.query.initStartTime) {
        vm.$delete(vm.query, 'StartTime')
      } else {
        vm.query.StartTime = vm.query.initStartTime + ' 00:00:00.000'
      }
      if (!vm.query.initEndTime) {
        vm.$delete(vm.query, 'EndTime')
      } else {
        vm.query.EndTime = vm.query.initEndTime + ' 23:59:59.999'
      }

      const exportquery = {}
      Object.assign(exportquery, vm.query)
      exportquery.CurrentPage = 0
      exportquery.PageSize = 999999
      debugger
      const myload = vm.inloading('导出中，请稍后......')
      communicate
        .TransformData(vm.init.table.datatype, exportquery)
        .then(response => {
          if (response[vm.init.table.listnode]) {
            const columns = []
            const headers = []
            const exist = []
            const data = convert.object2array(response[vm.init.table.listnode])
            this.$refs.table.columns.map(item => {
              if (
                item.property &&
                (item.type == 'default' || item.type == 'text') &&
                columns.indexOf(item.property) < 0 &&
                exist.indexOf(item.label) < 0
              ) {
                columns.push(item.property)
                headers.push(item.label)
              }
            })
            data.map(item => {
              //  日期格式化
              item.SignTime = this.parseTime(
                new Date(item.SignTime),
                '{y}-{m}-{d}'
              )
              // 状态格式化
              switch (item.Status) {
                case '0': {
                  item.Status = '签到'
                  break
                }
                case '1': {
                  item.Status = '签退'
                  break
                }
                default: {
                  item.Status = ''
                  break
                }
              }
            })
            excelhelper.export('到离站信息表', headers, columns, data)
          } else {
            vm.msgError('无数据')
          }
          vm.unloading(myload)
        }).catch(error => {
          console.log(error)
          vm.unloading(myload)
          this.msgError(this.errorMessage['Commmons']['1'])
        })
    }
  }
}
</script>

<style scoped>
.area {
  margin: 5px auto;
}
.label {
  text-align: center;
  line-height: 36px;
  font-size: 14px;
}
</style>
