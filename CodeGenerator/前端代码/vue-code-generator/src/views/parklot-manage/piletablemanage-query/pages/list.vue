<template>
  <div class="app-container">
    <!--  查询条件  -->
    <div class="area">
      <el-form ref="queryForm" :model="query" label-width="120px">
        <el-row>
          <el-col :span="6">
            <el-form-item label="充电站名">
              <el-select v-model="query.CStationName" style="width:210px;">
                <el-option
                  v-for="item in init.search.stations"
                  :key="item.key"
                  :label="item.label"
                  :value="item.value"
                />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="隶属部门">
              <el-select v-model="query.Department" style="width:210px;">
                <el-option key="all" label="--请选择--" value />
                <el-option key="suzhoutraffice" label="苏州公共交通有限公司" value="苏州公共交通有限公司" />
                <el-option key="xiangchengtraffice" label="苏州相城区公共交通有限公司" value="苏州相城区公共交通有限公司" />
                <el-option key="wuzhongbus" label="苏州市吴中区公共汽车有限公司" value="苏州市吴中区公共汽车有限公司" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="桩位号">
              <el-input v-model="query.PileNum" placeholder="请输入桩位号" />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="日期">
              <el-date-picker
                v-model="query.ReadingDate"
                type="date"
                format="yyyy-MM-dd"
                value-format="yyyy-MM-dd"
                placeholder="请选择时间"
                style="width:210px;"
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
      <el-button type="primary" size="small" class="el-button--export" @click="btnExport">
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
        <el-table-column
          width="100"
          label="日期"
          property="ReadingDate"
          align="center"
          :formatter="formatter"
        />
        <el-table-column width="200" label="隶属部门" property="Department" align="center" />
        <el-table-column width="200" label="充电站名" property="CStationName" align="center" />
        <el-table-column width="100" label="桩位号" property="PileNum" align="center" />
        <el-table-column width="100" label="上期读表" property="LastReading" align="center" />
        <el-table-column width="100" label="本期读表" property="NowReading" align="center" />
        <el-table-column width="100" label="电量倍率" property="PowerRatio" align="center" />
        <el-table-column label="耗电量" property="PowerConsumption" align="center" />
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
import excelhelper from '@/utils/excelhelper.js'
import convert from '@/utils/convert.js'

export default {
  name: 'List',
  components: {
  },
  data: function() {
    return {
      // 遮罩层
      loading: true,
      // 合计数量
      total: 0,
      // 时间区间
      Time: [],
      //  查询绑定参数
      query: {
        CurrentPage: 1,
        PageSize: 10,
        SortMethod: 'CStationName',
        OrderMethod: 'asc',
        CStationName: null,
        Department: null,
        PileNum: null,
        ReadingDate: null
      },
      //    初始化参数
      init: {
        //  查询条件初始化参数
        search: {
          stations: []
        },
        table: {
          height: window.innerHeight - 286,
          key: 'table',
          emptytext: '暂无数据',
          datatype: 'MeterReadingInfoList',
          listnode: 'MeterReading'
        },
        dialog: {
          width: '50%',
          showDialog: false,
          title: '',
          type: 'detail',
          id: null,
          piles: []
        },
        sort: {
          SortMethod: 'SLNO',
          OrderMethod: 'asc'
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
    this.getPileStations()
    this.getlist()
  },
  methods: {
    //  获取充电桩下拉菜单信息
    getPileStations: function() {
      const vm = this
      communicate
        .TransformData('ChargingPileInfoList', {
          SortMethod: 'CStationName',
          OrderMethod: 'asc',
          PageSize: '9999',
          CurrentPage: '1',
          CStationName: '',
          Department: '',
          StartDate: ''
        })
        .then(data => {
          vm.init.search.stations = []
          if (data.ChargingPile) {
            const sites = convert.object2array(data.ChargingPile)
            vm.init.dialog.piles = sites
            sites.map(item => {
              vm.init.search.stations.push({
                label: item.CStationName,
                value: item.CStationName,
                // value:
                //   item.PileNum +
                //   '&' +
                //   item.PowerRatio +
                //   '&' +
                //   item.Department,
                key: item.Guid
              })
            })
          }
          vm.init.search.stations.unshift({
            label: '--请选择--',
            value: '',
            key: 'empty'
          })
        })
        .catch(error => {
          console.log(error)
          this.msgError(this.errorMessage['Commmons']['1'])
        })
    },
    //  列format事件
    formatter: function(row, column, cellValue, index) {
      let value = null
      switch (column.label) {
        case '日期': {
          value = cellValue
            ? this.parseTime(new Date(cellValue), '{y}-{m}-{d}')
            : ''
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
      if (!vm.query.CStationName) {
        vm.$delete(vm.query, 'CStationName')
      }
      if (!vm.query.Department) {
        vm.$delete(vm.query, 'Department')
      }
      if (!vm.query.PileNum) {
        vm.$delete(vm.query, 'PileNum')
      }
      if (!vm.query.ReadingDate) {
        vm.$delete(vm.query, 'ReadingDate')
      }

      if (pager === undefined) {
        vm.query.CurrentPage = 0
      } else {
        vm.query.CurrentPage = pager.page - 1
        vm.query.PageSize = pager.limit
        vm.pager = pager
      }

      communicate
        .TransformData(vm.init.table.datatype, vm.query)
        .then(response => {
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
      this.pager.page = 1
      this.getlist()
    },
    //  导出按钮
    btnExport: function() {
      var vm = this

      //  接口对象处理
      if (!vm.query.CStationName) {
        vm.$delete(vm.query, 'CStationName')
      }
      if (!vm.query.Department) {
        vm.$delete(vm.query, 'Department')
      }
      if (!vm.query.PileNum) {
        vm.$delete(vm.query, 'PileNum')
      }
      if (!vm.query.ReadingDate) {
        vm.$delete(vm.query, 'ReadingDate')
      }

      const exportquery = {}
      Object.assign(exportquery, vm.query)
      exportquery.CurrentPage = 0
      exportquery.PageSize = 999999

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
              if (item.property && (item.type == 'default' || item.type == 'text') && columns.indexOf(item.property) < 0 && exist.indexOf(item.label) < 0) {
                columns.push(item.property)
                headers.push(item.label)
              }
            })
            data.map(item => {
              //  日期格式化
              item.ReadingDate = this.parseTime(new Date(item.ReadingDate), '{y}-{m}-{d}')
            })
            excelhelper.export('充电站抄表信息表', headers, columns, data)
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
