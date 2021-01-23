<template>
  <div class="app-container">
    <!--  查询条件  -->
    <div class="area">
      <el-form ref="queryForm" :model="query" label-width="120px">
        <el-row>
          <el-col :span="8">
            <el-form-item label="开始时间">
              <el-date-picker
                v-model="query.initStartTime"
                type="date"
                format="yyyy-MM-dd"
                value-format="yyyy-MM-dd"
                placeholder="请选择开始时间"
                style="width:210px;"
                :picker-options="pickerOptionsStart"
                @change="changeEnd"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="结束时间">
              <el-date-picker
                v-model="query.initEndTime"
                type="date"
                format="yyyy-MM-dd"
                value-format="yyyy-MM-dd"
                placeholder="请选择结束时间"
                style="width:210px;"
                :picker-options="pickerOptionsEnd"
                @change="changeStart"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="驾驶员姓名">
              <el-input v-model="query.DriverName" placeholder="请输入驾驶员姓名" style="width:210px;" />
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="车牌号">
              <el-input v-model="query.DBusCard" placeholder="请输入车牌号" style="width:210px;" />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="企业自编号">
              <el-input v-model="query.BusNo" placeholder="请输入企业自编号" style="width:210px;" />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="刷卡数据类型">
              <el-select v-model="query.ICType" style="width:210px;">
                <el-option key="all" label="全部" value />
                <el-option key="login" label="登录" value="0" />
                <el-option key="logout" label="注销" value="1" />
              </el-select>
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
        <el-table-column width="180" label="车辆车牌号" property="DBusCard" align="center" />
        <el-table-column width="180" label="企业自编号" property="BusNo" align="center" />
        <el-table-column width="200" label="日期时间" property="LUTC" align="center" />
        <el-table-column width="200" label="驾驶员姓名" property="DriverName" align="center" />
        <el-table-column label="驾驶员刷卡数据类型" property="ICType" align="center" :formatter="formatter" />
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
import request from '@/utils/action.js'
import communicate from '@/utils/communicate.js'
import { baseinfo, userinfo } from '@/utils/infohelper.js'
import excelhelper from '@/utils/excelhelper.js'
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
        SortMethod: 'LUTC',
        OrderMethod: 'desc',
        initStartTime: null,
        initEndTime: null,
        StartTime: null,
        EndTime: null,
        DriverName: null,
        ICType: null,
        DBusCard: null,
        BusNo: null
      },
      //    初始化参数
      init: {
        //  查询条件初始化参数
        search: {},
        table: {
          height: window.innerHeight - 366,
          key: 'table',
          emptytext: '暂无数据',
          datatype: 'ICDataList',
          listnode: 'ICData'
        },
        dialog: {
          width: '50%',
          showDialog: false,
          title: '',
          type: 'detail',
          Id: null
        },
        sort: {
          SortMethod: 'LUTC',
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
    // this.init.table.height =
    //   window.innerHeight - this.$refs.table.$el.offsetTop - 50;
    //  console.log(this.init.table.height);
    //  this.getlist()
    this.initData()
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
    initData() {
      this.query.initStartTime = this.helpers.date.fromToday(-1)
      this.query.initEndTime = this.helpers.date.fromToday(-1)
    },
    //  列format事件
    formatter: function(row, column, cellValue, index) {
      let value = null
      switch (column.label) {
        case '驾驶员刷卡数据类型': {
          switch (cellValue) {
            case '0': {
              value = '登录'
              break
            }
            case '1': {
              value = '注销'
              break
            }
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
      if (!vm.query.ICType) {
        vm.$delete(vm.query, 'ICType')
      }
      if (!vm.query.DriverName) {
        vm.$delete(vm.query, 'DriverName')
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
      debugger
      if (!vm.query.DBusCard) {
        vm.$delete(vm.query, 'DBusCard')
      }
      if (!vm.query.BusNo) {
        vm.$delete(vm.query, 'BusNo')
      }
      if (!vm.query.ICType) {
        vm.$delete(vm.query, 'ICType')
      }
      if (!vm.query.DriverName) {
        vm.$delete(vm.query, 'DriverName')
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

      const myload = vm.inloading('导出中，请稍后......')
      communicate
        .TransformData(vm.init.table.datatype, exportquery, 'cardata', 'cardata')
        .then(response => {
          if (response[vm.init.table.listnode]) {
            const columns = []
            const headers = []
            const exist = []
            const data = convert.object2array(response[vm.init.table.listnode])
            this.$refs.table.columns.map(item => {
              if (item.property && (item.type == 'default' || item.type == 'text') && columns.indexOf(item.property) < 0 && exist.indexOf(item.label) < 0) {
                columns.push(item.property)
                headers.push(item.label)
              }
            })
            data.map(item => {
              // 状态格式化
              switch (item.ICType) {
                case '0': {
                  item.ICType = '登录'
                  break
                } case '1': {
                  item.ICType = '注销'
                  break
                } default: {
                  item.ICType = ''
                  break
                }
              }
            })
            excelhelper.export('车载登录注销记录表', headers, columns, data)
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
