<template>
  <div class="app-container">
    <!--  查询条件  -->
    <div class="area">
      <el-form ref="queryForm" :model="query" label-width="120px">
        <el-row>
          <el-col :span="8">
            <el-form-item label="统计日期">
              <el-date-picker
                v-model="query.StatDate"
                type="date"
                format="yyyy-MM-dd"
                value-format="yyyy-MM-dd"
                placeholder="请选择统计日期"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="公司">
              <el-select v-model="query.StatComp">
                <el-option key="all" label="全部" value />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="部门">
              <el-select v-model="query.StatDept">
                <el-option key="all" label="全部" value />
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
    <label>明细 :</label>
    <div class="area">
      <el-table
        :ref="init.table.key"
        :key="init.table.key"
        v-loading="loading"
        :border="true"
        :data="data"
        style="width:100%;"
        :fit="true"
        :stripe="true"
        :empty-text="init.table.emptytext"
      >
        <el-table-column :label="$t('detail.index')" align="center" width="80">
          <template scope="scope"><span>{{ scope.$index+(query.CurrentPage) * query.PageSize + 1 }} </span></template>
        </el-table-column>
        <el-table-column width="180" label="车号" property="ch" align="center" />
        <el-table-column width="180" label="上存" property="sc" align="center" />
        <el-table-column width="180" label="加油" property="jy" align="center" />
        <el-table-column width="180" label="保养用油" property="byyy" align="center" />
        <el-table-column width="180" label="耗油" property="hy" align="center" />
        <el-table-column width="180" label="存油" property="cy" align="center" />
        <el-table-column width="180" label="节费" property="jf" align="center" />
      </el-table>

      <pagination
        v-show="total>0"
        :total="total"
        :page.sync="pager.page"
        :limit.sync="pager.limit"
        @pagination="getlist"
      />
    </div>

    <label>汇总 :</label>
    <div class="area">
      <el-table
        :ref="init.table2.key"
        :key="init.table2.key"
        v-loading="loading"
        :border="true"
        :data="data2"
        style="width:100%;"
        :fit="true"
        :stripe="true"
        :empty-text="init.table2.emptytext"
        :height="init.table2.height"
      >
        <el-table-column type="index" label="序号" align="center" width="80" />
        <el-table-column width="180" label="车号" property="ch" align="center" fixed="left" />
        <el-table-column width="180" label="上存" property="sc" align="center" />
        <el-table-column width="180" label="加油" property="jy" align="center" />
        <el-table-column width="180" label="保养用油" property="byyy" align="center" />
        <el-table-column width="180" label="耗油" property="hy" align="center" />
        <el-table-column width="180" label="存油" property="cy" align="center" />
        <el-table-column width="180" label="节费" property="jf" align="center" />
      </el-table>
    </div>
  </div>
</template>

<script>
import communicate from '@/utils/communicate.js'
import excelhelper from '@/utils/excelhelper.js'
import convert from '@/utils/convert.js'

export default {
  name: 'List',
  components: {},
  data: function() {
    return {
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
        StatDate: null,
        StatComp: null,
        StatDept: null
      },
      //    初始化参数
      init: {
        //  查询条件初始化参数
        search: {},
        table: {
          height: window.innerHeight - 250,
          key: 'table',
          emptytext: '暂无数据',
          datatype: 'OABusOilList',
          listnode: 'OABusOil'
        },
        table2: {
          height: 300,
          key: 'table2',
          emptytext: '暂无数据',
          datatype: 'OABusOilList',
          listnode: 'OABusOilOther'
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
      data2: [],
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
    this.getlist()
  },
  methods: {
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
      // if (this.$refs[this.init.table.key]) this.$refs[this.init.table.key].bodyWrapper.scrollTop = 0
      // if (this.$refs[this.init.table2.key]) this.$refs[this.init.table2.key].bodyWrapper.scrollTop = 0
      vm.loading = true

      if (pager === undefined) {
        vm.query.CurrentPage = 0
      } else {
        vm.query.CurrentPage = pager.page - 1
        vm.query.PageSize = pager.limit
        vm.pager = pager
      }

      communicate
        .TransformData(vm.init.table.datatype, vm.helpers.convert.null2empty(vm.query), 'report', 'report')
        .then(response => {
          vm.data = []
          console.log('response : ', response)
          if (response[vm.init.table.listnode]) {
            vm.data = vm.helpers.convert.object2array(response[vm.init.table.listnode])
          }
          if (response[vm.init.table2.listnode]) {
            vm.data2 = vm.helpers.convert.object2array(response[vm.init.table2.listnode])
          }
          vm.total = response._Total
          vm.loading = false
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

      const exportquery = {}
      Object.assign(exportquery, vm.helpers.convert.null2empty(vm.query))
      exportquery.CurrentPage = 0
      exportquery.PageSize = 999999

      const myload = vm.inloading('导出中，请稍后......')
      communicate
        .TransformData(vm.init.table.datatype, exportquery, 'report', 'report')
        .then(response => {
          if (response[vm.init.table.listnode]) {
            const columns = []
            const headers = []
            const data = convert.object2array(response[vm.init.table.listnode])
            const exist = []
            this.$refs.table.columns.map(item => {
              if (item.property && (item.type == 'default' || item.type == 'text') && columns.indexOf(item.property) < 0 && exist.indexOf(item.label) < 0) {
                columns.push(item.property)
                headers.push(item.label)
              }
            })

            excelhelper.export('车辆用油情况表--明细', headers, columns, data)
          } else {
            vm.msgError('无数据')
          }
          if (response[vm.init.table2.listnode]) {
            const columns = []
            const headers = []
            const data = convert.object2array(response[vm.init.table2.listnode])
            const exist = []
            this.$refs.table2.columns.map(item => {
              if (item.property && (item.type == 'default' || item.type == 'text') && columns.indexOf(item.property) < 0 && exist.indexOf(item.label) < 0) {
                columns.push(item.property)
                headers.push(item.label)
              }
            })

            excelhelper.export('车辆用油情况表--汇总', headers, columns, data)
          }
          vm.unloading(myload)
        }).catch(error => {
          console.log(error)
          vm.unloading(myload)
          this.msgError(this.errorMessage['Commmons']['1'])
        })
    },

    // json转换
    formatJson(filterVal, jsonData) {
      return jsonData.map(v =>
        filterVal.map(j => {
          if (j === 'timestamp') {
            return this.parseTime(v[j])
          } else {
            return v[j]
          }
        })
      )
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
