<template>
  <div class="app-container">
    <!--  查询条件  -->
    <el-form ref="queryForm" :model="listQuery" :inline="true">
      <el-form-item label="日期">
        <el-date-picker
          v-model="dateRange"
          size="small"
          style="width: 240px"
          value-format="yyyy-MM-dd"
          type="daterange"
          range-separator="-"
          :picker-options="pickerOptions"
          start-placeholder="开始日期"
          end-placeholder="结束日期"
        />
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
      <div id="pieReport" style="height: 400px;align:center" />
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
          prop="CompName"
          :label="$t('detail.company')"
          :resizable="true"
          align="center"
        />
        <el-table-column prop="TypeName" width="180" label="报警类型" :resizable="true" align="center" />
        <el-table-column prop="Num" width="180" label="报警数量" :resizable="true" align="center" />
        <el-table-column prop="DoneNum" width="180" label="未处理数量" :resizable="true" align="center" />
      </el-table>

      <pagination
        v-show="total>0"
        :total="total"
        :page.sync="listQuery.CurrentPage"
        :limit.sync="listQuery.PageSize"
        @pagination="getlist"
      />
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
      loading: true,
      // 合计数量
      total: 0,
      //  查询绑定参数
      listQuery: {
        CurrentPage: 1,
        PageSize: 10,
        SortMethod: 'CompName'
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
        table: {
          height: window.innerHeight - 300,
          key: 'table',
          emptytext: '暂无数据',
          datatype: 'QueryDailyAlarmDoneSummary',
          datatypechart: 'QueryDailyAlarmDoneChart',
          url: 'http://localhost:8016/User/GetList',
          listnode: 'Data',
          type: 'post'
        }
      },
      charts: '',
      opinion: [],
      opinionData: [],
      opinionNData: [],
      //    表格数据
      data: []
    }
  },
  mounted: function() {
    //  初始化表格数据
    this.getlist()
  },
  methods: {
    //  获取数据
    getlist: function() {
      var vm = this
      if (this.$refs[this.init.table.key]) this.$refs[this.init.table.key].bodyWrapper.scrollTop = 0
      vm.loading = true
      if (!vm.dateRange) {
        vm.$delete(vm.listQuery, 'StartTime')
        vm.$delete(vm.listQuery, 'EndTime')
      }
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
            vm.loading = false
          } else {
            vm.data = []
            vm.total = 0
            vm.loading = false
          }
        })
      communicate
        .TransformData(vm.init.table.datatypechart, this.addDateRange(vm.listQuery, vm.dateRange), 'busCompany', 'business')
        .then(response => {
          vm.opinion = []
          vm.opinionData = []
          vm.opinionNData = []
          if (response[vm.init.table.listnode]) {
            if (Array.isArray(response[vm.init.table.listnode])) {
              vm.opinionData = response[vm.init.table.listnode]
            } else {
              vm.opinionData.push(response[vm.init.table.listnode])
            }
            vm.opinionData.forEach(function(item, index) {
              var obj = {}
              obj.name = item.name
              obj.value = item.nvalue
              vm.opinion.push(obj.name)
              vm.opinionNData.push(obj)
            })
          }
          this.drawPie('pieReport')
        })
    },
    //  查询按钮
    btnlistQuery: function() {
      this.listQuery.CurrentPage = 1
      this.getlist()
    },
    btnExport: function() {
      var vm = this
      //  接口对象处理
      if (!vm.dateRange) {
        vm.$delete(vm.listQuery, 'StartTime')
        vm.$delete(vm.listQuery, 'EndTime')
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
            excelhelper.export('报警处理统计', headers, columns, data)
          } else {
            vm.msgError('无数据')
          }
          vm.unloading(myload)
        })
    },
    drawPie(id) {
      this.charts = echarts.init(document.getElementById(id))
      this.charts.setOption({
        tooltip: {
          trigger: 'item',
          formatter: '{a}<br/>{b}:{c} ({d}%)'
        },
        title: [{
          text: '未完成数量',
          left: '25%',
          top: '8%',
          textAlign: 'center'
        }, {
          text: '所有数量',
          left: '75%',
          top: '8%',
          textAlign: 'center'
        }],
        legend: {
          top: 'bottom',
          left: 'center',
          data: this.opinion
        },
        series: [
          {
            name: '未完成数量',
            type: 'pie',
            radius: '65%',
            center: ['25%', '50%'],
            itemStyle: {
              emphasis: {
                shadowBlur: 10,
                shadowOffsetX: 0,
                shadowColor: 'rgba(0, 0, 0, 0.5)'
              }
            },
            label: {
              normal: {
                show: true,
                formatter: '{b}:{c}'
              }
            },
            data: this.opinionData
          },
          {
            name: '所有数量',
            type: 'pie',
            radius: '65%',
            center: ['75%', '50%'],
            itemStyle: {
              emphasis: {
                shadowBlur: 10,
                shadowOffsetX: 0,
                shadowColor: 'rgba(0, 0, 0, 0.5)'
              }
            },
            label: {
              normal: {
                show: true,
                formatter: '{b}:{c}'
              }
            },
            data: this.opinionNData
          }
        ]
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
