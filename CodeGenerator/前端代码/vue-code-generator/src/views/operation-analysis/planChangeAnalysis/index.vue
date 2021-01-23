<template>
  <div class="app-container">
    <!--  查询条件  -->
    <el-form ref="queryForm" :model="listQuery" :inline="true">
      <el-form-item label="创建时间">
        <el-date-picker
          v-model="dateRange"
          size="small"
          style="width: 240px"
          value-format="yyyy-MM-dd"
          :picker-options="pickerOptions"
          type="daterange"
          range-separator="-"
          start-placeholder="开始日期"
          end-placeholder="结束日期"
        />
      </el-form-item>
      <el-form-item label="调度类型" prop="status">
        <el-input v-model="listQuery.AdjustReason" placeholder="请输入调度类型" style="width:200px;" />
      </el-form-item>
      <el-form-item :label="$t('search.company')" prop="status">
        <el-select
          v-model="listQuery.CompName"
          :filterable="true"
          :placeholder="$t('ddllist.pleaseselect')"
        >
          <el-option
            v-for="item in init.search.corp"
            :key="item.value"
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
      <div id="pieReport" style="height: 500px;align:center" />
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
          property="CompName"
          :label="$t('detail.company')"
          :resizable="true"
          align="center"
        />
        <el-table-column
          property="Date"
          width="180"
          :label="$t('detail.date')"
          :resizable="true"
          align="center"
        />
        <el-table-column property="AdjustReason" width="280" label="调度类型" :resizable="true" align="center" />
        <el-table-column prop="Num" width="180" :label="$t('detail.count')" :resizable="true" align="center">
          <template slot-scope="scope">
            <div style="color:red;text-decoration:underline;cursor:pointer;" @click="clickDetail(scope.row)">{{ scope.row.Num }}</div>
          </template>
        </el-table-column>
      </el-table>

      <pagination
        v-show="total>0"
        :total="total"
        :page.sync="listQuery.CurrentPage"
        :limit.sync="listQuery.PageSize"
        @pagination="getlist"
      />
      <el-dialog title="计划详情" :visible.sync="open" width="1000px" height="500px">
        <el-table
          :ref="init.table.key"
          :key="init.table.key"
          v-loading="detailloading"
          :border="true"
          :data="detaildata"
          style="width:100%;"
          :height="init.table.height"
          :fit="true"
          :stripe="true"
          :empty-text="init.table.emptytext"
        >
          <el-table-column :label="$t('detail.index')" align="center" width="70">
            <template scope="scope"><span>{{ scope.$index+(detailQuery.CurrentPage - 1) * detailQuery.PageSize + 1 }} </span></template>
          </el-table-column>
          <el-table-column prop="LName" width="180" label="线路番号" :resizable="true" align="center" />
          <el-table-column prop="AdjustReason" width="180" label="调度类型" :resizable="true" align="center" />
          <el-table-column prop="Editor" label="调整人" :resizable="true" align="center" />
          <el-table-column prop="AdjustTime" width="180" label="调整时间" :resizable="true" align="center" />
        </el-table>

        <pagination
          v-show="detailtotal>0"
          :total="detailtotal"
          :page.sync="detailQuery.CurrentPage"
          :limit.sync="detailQuery.PageSize"
          @pagination="getDetail"
        />
      </el-dialog>
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
      pickerOptions: {
        disabledDate(time) {
          return time.getTime() > Date.now() - 8.64e6
        }
      },
      // 遮罩层
      loading: true,
      detailloading: false,
      // 是否显示弹出层
      open: false,
      // 合计数量
      total: 0,
      detailtotal: 0,
      //  查询绑定参数
      listQuery: {
        CurrentPage: 1,
        PageSize: 10,
        SortMethod: 'CompName'
      },
      detailQuery: {
        CurrentPage: 1,
        PageSize: 10,
        SortMethod: 'LName'
      },
      // 日期范围
      dateRange: [],
      // 初始化参数
      init: {
        //  查询条件初始化参数
        search: {
          corp: [
            { value: '', label: '-请选择-' },
            { value: '苏州公共交通有限公司', label: '苏州市公交' },
            { value: '苏州相城区公共交通有限公司', label: '相城公交' },
            { value: '苏州市吴中区公共汽车有限公司', label: '吴中公交' }]
        },
        table: {
          height: window.innerHeight - 420,
          key: 'table',
          emptytext: '暂无数据',
          url: 'http://localhost:8016/User/GetList',
          type: 'post',
          datatype: 'QueryDailyPlanChangeCompany',
          datatypechart: 'QueryDailyPlanChangeCompanySummary',
          datadetailtype: 'QueryDailyPlanChangeDetail',
          listnode: 'Data'
        }
      },
      charts: '',
      opinion: [],
      opinionData: [],
      //    表格数据
      pie: false,
      line: false,
      allline: 0,
      data: [],
      detaildata: []
    }
  },
  mounted: function() {
    this.$nextTick(function() {
      this.getlist()
    })
  },
  methods: {
    //  获取数据
    getlist: function() {
      var vm = this
      if (this.$refs[this.init.table.key]) this.$refs[this.init.table.key].bodyWrapper.scrollTop = 0
      vm.loading = true
      if (!vm.listQuery.CompName) {
        vm.$delete(vm.listQuery, 'CompName')
      }
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
          if (response[vm.init.table.listnode]) {
            if (Array.isArray(response[vm.init.table.listnode])) {
              vm.opinionData = response[vm.init.table.listnode]
            } else {
              vm.opinionData.push(response[vm.init.table.listnode])
            }
            vm.opinionData.forEach(function(item, index) {
              var obj = {}
              obj.name = item.name
              vm.opinion.push(obj.name)
            })
            vm.allline = response._Total
          }
          if (vm.listQuery.CompName) {
            this.drawLine('pieReport')
          } else {
            this.drawPie('pieReport')
          }
        })
    },
    clickDetail(row) {
      this.getDetilQuery(row)
      this.getDetail()
    },
    getDetilQuery(row) {
      this.open = true
      this.detailQuery.CurrentPage = 1
      this.detailQuery.Date = row.Date
      if (row.CompName) {
        this.detailQuery.CompName = row.CompName
      }
      if (row.AdjustReason) {
        this.detailQuery.AdjustReason = row.AdjustReason
      }
    },
    //  获取数据
    getDetail: function() {
      var vm = this
      vm.detailloading = true
      if (!vm.detailQuery.CompName) {
        vm.$delete(vm.detailQuery, 'CompName')
      }
      if (!vm.detailQuery.AdjustReason) {
        vm.$delete(vm.detailQuery, 'AdjustReason')
      }
      if (!vm.dateRange) {
        vm.$delete(vm.detailQuery, 'StartTime')
        vm.$delete(vm.detailQuery, 'EndTime')
      }
      communicate
        .TransformData(vm.init.table.datadetailtype, vm.detailQuery, 'busCompany', 'business')
        .then(response => {
          vm.detaildata = []
          if (response[vm.init.table.listnode]) {
            if (Array.isArray(response[vm.init.table.listnode])) {
              vm.detaildata = response[vm.init.table.listnode]
            } else {
              vm.detaildata.push(response[vm.init.table.listnode])
            }
            vm.detailtotal = response._Total
            vm.detailloading = false
          } else {
            vm.detaildata = []
            vm.detailtotal = 0
            vm.detailloading = false
          }
        })
    },
    //  查询按钮
    btnlistQuery: function() {
      this.listQuery.CurrentPage = 1
      this.getlist()
    },
    // 重置按钮操作
    resetQuery() {
      this.dateRange = []
      this.resetForm('queryForm')
      this.btnlistQuery()
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
            excelhelper.export('计划调整统计', headers, columns, data)
          } else {
            vm.msgError('无数据')
          }
          vm.unloading(myload)
        })
    },
    drawPie(id) {
      this.charts = echarts.init(document.getElementById(id))
      this.charts.clear()
      this.charts.setOption({
        tooltip: {
          trigger: 'item',
          formatter: '{a}<br/>{b}:{c} ({d}%)'
        },
        legend: {
          type: 'scroll',
          orient: 'vertical',
          right: 10,
          top: 20,
          bottom: 20,
          data: this.opinion
        },
        series: [
          {
            name: '数量',
            type: 'pie',
            radius: '65%',
            center: ['50%', '50%'],
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
          }
        ]
      })
    },
    drawLine(id) {
      this.charts = echarts.init(document.getElementById(id))
      this.charts.clear()
      var all = this.allline
      this.charts.setOption({
        legend: {
          data: this.opinion
        },
        tooltip: {
          trigger: 'axis',
          formatter: function(params) {
            var html = ''
            for (var i = 0; i < params.length; i++) {
              html += '<span style="display:inline-block;margin-right:5px;border-radius:10px;width:10px;height:10px;background-color:' + params[i].color + ';"></span>'

              html += params[i].seriesName + ':' + params[i].value + '(' + (params[i].value / all * 100).toFixed(2) + '%)<br>'
            }
            return html
          }

        },
        xAxis: {
          type: 'category',
          data: this.opinion,
          axisLabel: {
            interval: 0,
            rotate: 40,
            formatter: function(name) {
              return (name.length > 6 ? (name.slice(0, 6) + '...') : name)
            }
          }
        },
        yAxis: {
          type: 'value'
        },
        series: [{
          name: '数量',
          data: this.opinionData,
          type: 'line'
        }]
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
