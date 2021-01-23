<template>
  <div class="app-container">
    <!--  查询条件  -->
    <el-form ref="queryForm" :model="listQuery" :inline="true">
      <el-form-item label="日期">
        <el-date-picker
          v-model="dateRange"
          size="small"
          style="width: 240px "
          value-format="yyyy-MM-dd"
          type="daterange"
          range-separator="-"
          :picker-options="pickerOptions"
          start-placeholder="开始日期"
          end-placeholder="结束日期"
        />
      </el-form-item>
      <el-form-item label="线路番号">
        <el-select v-model="listQuery.LName" style="width:210px;">
          <el-option
            v-for="item in init.search.lineNumber"
            :key="item.key"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item>
        <el-button type="primary" icon="el-icon-search" size="mini" @click="btnlistQuery">搜索</el-button>
        <el-button type="info" size="small" class="el-button--export" @click="btnExport">
          <i class="el-icon-download" />
          导出
        </el-button>
      </el-form-item>
    </el-form>
    <!--  图标  -->
    <div class="area">
      <el-table
        :ref="init.table.key"
        :key="init.table.key"
        v-loading="loading"
        :border="true"
        :data="data"
        style="width:100%;"
        :height="init.table.height"
        :fit="true"
        :stripe="true"
        :empty-text="init.table.emptytext"
      >
        <el-table-column :label="$t('detail.index')" align="center" width="70">
          <template scope="scope"><span>{{ scope.$index+(listQuery.CurrentPage - 1) * listQuery.PageSize + 1 }} </span></template>
        </el-table-column>
        <el-table-column
          prop="Date"
          width="180"
          :label="$t('detail.date')"
          :resizable="true"
          align="center"
        />
        <el-table-column
          prop="LName"
          label="线路"
          :resizable="true"
          align="center"
        />
        <el-table-column prop="Num" width="180" label="总班次数" :resizable="true" align="center" />
        <el-table-column prop="DoneNum" width="180" label="准点数量" :resizable="true" align="center" />
        <el-table-column prop="Percent" width="180" label="准点比例" :resizable="true" align="center" />
      </el-table>

      <pagination
        v-show="total>0"
        :total="total"
        :page.sync="listQuery.CurrentPage"
        :limit.sync="listQuery.PageSize"
        @pagination="getlist"
      />
      <div id="pieReport" ref="chart" style="height: 500px;align:center" />
    </div>
  </div>
</template>

<script>
import echarts from 'echarts'
import communicate from '@/utils/communicate.js'
import excelhelper from '@/utils/excelhelper.js'
import convert from '@/utils/convert.js'

export default {
  name: 'List',
  data: function() {
    return {
      // 遮罩层
      loading: false,
      lineshow: false,
      // 合计数量
      total: 0,
      //  查询绑定参数
      listQuery: {
        CurrentPage: 1,
        PageSize: 10,
        SortMethod: 'LName'
      },
      pickerOptions: {
        disabledDate(time) {
          return time.getTime() > Date.now() - 8.64e6
        }
      },
      // 日期范围
      dateRange: [],
      // 初始化参数
      init: {
        //  查询条件初始化参数
        search: {
          lineNumber: [
            {
              key: 'empty',
              value: '',
              label: '--请选择--'
            },
            {
              key: '1',
              value: '1',
              label: '1'
            },
            {
              key: '10',
              value: '10',
              label: '10'
            },
            {
              key: '900',
              value: '900',
              label: '900'
            }
          ],
          corp: [],
          line: [],
          shift: []
        },
        table: {
          height: window.innerHeight - 300,
          key: 'table',
          emptytext: '暂无数据',
          datatype: 'QueryDailyOnTimeLine',
          url: 'http://localhost:8016/User/GetList',
          listnode: 'Data',
          type: 'post'
        }
      },
      //    表格数据
      data: [],
      charts: '',
      opinion: [],
      opinionData: []
    }
  },
  methods: {
    //  获取数据
    getlist: function() {
      var vm = this
      if (this.$refs[this.init.table.key]) this.$refs[this.init.table.key].bodyWrapper.scrollTop = 0
      if (vm.dateRange.length === 0) {
        this.msgError('请选择日期范围！')
        return
      }
      vm.loading = true
      communicate
        .TransformData(vm.init.table.datatype, this.addDateRange(vm.listQuery, vm.dateRange), 'busCompany', 'business')
        .then(response => {
          vm.data = []
          if (response[vm.init.table.listnode]) {
            if (Array.isArray(response[vm.init.table.listnode])) {
              vm.data = response[vm.init.table.listnode]
            } else {
              vm.data.push(response[vm.init.table.listnode])
            }
            vm.total = response._Total
            if (!vm.listQuery.LName) {
              vm.lineshow = false
            } else {
              vm.lineshow = true
              vm.opinion = []
              vm.opinionData = []
              vm.data.forEach(function(item, index) {
                var obj = {}
                obj.value = item.Percent
                obj.name = item.Date
                vm.opinion.push(obj.name)
                vm.opinionData.push(obj)
              })
              this.drawLine('pieReport')
            }
            vm.loading = false
          } else {
            vm.data = []
            vm.total = 0
            vm.loading = false
          }
        })
    },
    //  查询按钮
    btnlistQuery: function() {
      this.listQuery.CurrentPage = 1
      this.getlist()
    },
    drawLine(id) {
      console.log(this.opinionData)
      this.charts = echarts.init(document.getElementById(id))
      this.charts.setOption({
        title: {
          text: '线路准点统计图', left: 'center'
        },
        legend: {
          data: this.opinion
        },
        tooltip: {
          trigger: 'axis'
        },
        dataZoom: [
          {
            show: true,
            realtime: true
          }
        ],
        xAxis: {
          type: 'category',
          data: this.opinion,
          axisLabel: {
            interval: 0
          }
        },
        yAxis: {
          type: 'value'
        },
        series: [{
          name: '比例',
          data: this.opinionData,
          type: 'line'
        }]
      })
    },
    btnExport: function() {
      var vm = this
      //  接口对象处理
      if (!vm.dateRange) {
        this.$msgError('请选择日期f范围！')
        return
      }

      const exportquery = {}
      Object.assign(exportquery, vm.listQuery)
      exportquery.CurrentPage = 0
      exportquery.PageSize = 999999

      const myload = vm.inloading('导出中，请稍后......')
      communicate
        .TransformData(vm.init.table.datatype, this.addDateRange(exportquery, vm.dateRange), 'busCompany', 'business')
        .then(response => {
          if (response[vm.init.table.listnode]) {
            const columns = []
            const headers = []
            const exist = ['操作']
            const data = convert.object2array(response[vm.init.table.listnode])
            console.log(this.$refs.table.columns)
            this.$refs.table.columns.map(item => {
              if (item.property && (item.type === 'default' || item.type === 'text') && columns.indexOf(item.property) < 0 && exist.indexOf(item.label) < 0) {
                columns.push(item.property)
                headers.push(item.label)
              }
            })
            excelhelper.export('线路准点统计', headers, columns, data)
          } else {
            vm.msgError('无数据')
          }
          vm.unloading(myload)
        })
    }
  }
}
</script>

<style scoped>
.area {
  margin: 5px auto;
}
.el-row {
  margin: 5px auto;
}

.label {
  text-align: center;
  line-height: 36px;
  font-size: 14px;
}
</style>
