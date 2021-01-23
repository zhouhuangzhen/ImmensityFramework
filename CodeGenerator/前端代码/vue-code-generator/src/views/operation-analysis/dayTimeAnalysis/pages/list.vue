<template>
  <div class="app-container">
    <!--  查询条件  -->
    <el-row>
      <el-col :span="2" class="label" v-text="$t('search.startdate')" />
      <el-col :span="4">
        <el-date-picker v-model="listQuery.startdate" type="date" />
      </el-col>
      <el-col :span="2" class="label" v-text="$t('search.enddate')" />
      <el-col :span="4">
        <el-date-picker v-model="listQuery.enddate" type="date" />
      </el-col>
      <el-col :span="2" class="label" v-text="$t('search.atttype')" />
      <el-col :span="4">
        <el-select v-model="listQuery.atttype" :placeholder="$t('ddllist.pleaseselect')">
          <el-option key="all" :label="$t('ddllist.all')" value />
          <el-option key="checkin" :label="$t('ddllist.checkin')" value="1" />
          <el-option key="checkout" :label="$t('ddllist.checkout')" value="2" />
        </el-select>
      </el-col>
      <el-col :span="2" class="label" v-text="$t('search.company')" />
      <el-col :span="4">
        <el-select v-model="listQuery.corp" :filterable="true" :placeholder="$t('ddllist.pleaseselect')">
          <el-option
            v-for="item in init.search.corp"
            :key="item.value"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-col>
    </el-row>
    <!--  按钮区域  -->
    <div style="text-align:right;" class="area">
      <el-button type="primary" size="small" @click="btnlistQuery">
        <i class="el-icon-upload" />
        查询
      </el-button>
    </div>
    <!--  图标  -->
    <div class="area">
      <el-table
        :ref="init.table.key"
        :key="init.table.key"
        :border="true"
        :data="data"
        style="width:100%;"
        :fit="true"
        :height="init.table.height"
        :stripe="true"
        :empty-text="init.table.emptytext"
        @select="select"
        @select-all="selectall"
        @cell-click="cellclick"
        @row-click="rowclick"
      >
        <el-table-column :label="$t('detail.index')" align="center" width="70">
          <template scope="scope"><span>{{ scope.$index+(listQuery.CurrentPage - 1) * listQuery.PageSize + 1 }} </span></template>
        </el-table-column>
        <el-table-column prop="name" width="180" :label="$t('detail.date')" :resizable="true" align="center" />
        <el-table-column prop="name" :label="$t('detail.company')" :resizable="true" align="center" />
        <el-table-column prop="name" width="180" label="调度类型" :resizable="true" align="center" />
        <el-table-column prop="name" width="180" :label="$t('detail.count')" :resizable="true" align="center" />
      </el-table>

      <el-pagination
        layout="prev, pager, next,sizes,total,jumper"
        :background="true"
        :page-size="pager.size"
        :current-page="pager.index"
        :page-sizes="pager.sizes"
        :total="pager.total"
        @size-change="sizechange"
        @current-change="currentchange"
        @prev-click="prev"
        @next-click="next"
      />
      <div id="pieReport" style="width: 400px;height: 300px;text-align:center" />
    </div>
  </div>
</template>

<script>
import echarts from 'echarts'
import request from '@/utils/request.js'

export default {
  name: 'List',
  data: function() {
    return {
      //  查询绑定参数
      listQuery: {
        type: null,
        corp: null,
        line: null,
        shift: null,
        pageIndex: 1,
        pageSize: 1,
        sort: ''
      },
      //    初始化参数
      init: {
        //  查询条件初始化参数
        search: {
          corp: [],
          line: [],
          shift: []
        },
        table: {
          height: window.innerHeight - 420,
          key: 'table',
          emptytext: '暂无数据',
          url: 'http://localhost:8016/User/GetList',
          type: 'post'
        }
      },
      charts: '',
      opinion: ['及格人数', '未及格人数'],
      opinionData: [
        { value: 12, name: '及格人数', itemStyle: '#1ab394' },
        { value: 18, name: '未及格人数', itemStyle: '#79d2c0' }
      ],
      //    表格数据
      data: [],
      //    分页控件参数
      pager: {
        size: 10,
        index: 1,
        sizes: [10, 20, 30, 40, 50],
        total: 0
      }
    }
  },
  mounted: function() {
    this.$nextTick(function() {
      this.drawPie('pieReport')
    })
  },
  methods: {
    //  选中事件
    select: function(selection, row) {},
    //  全选事件
    selectall: function(selection) {},
    //  单元格点击事件
    cellclick: function(row, column, cell, event) {},
    //  行点击事件
    rowclick: function(row, column, event) {},
    //  列format事件
    formatter: function(row, column, cellValue, index) {},
    //  页数量变化事件
    sizechange: function(size) {
      this.pager.size = size
      this.getlist()
    },
    //  页变化事件
    currentchange: function(value) {
      this.pager.index = value
      this.getlist()
    },
    //  上一页
    prev: function(value) {
      this.pager.index = value
    },
    //  下一页
    next: function(value) {
      this.pager.index = value
    },
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
    //  是否刷新数据
    refresh: function(bRefresh) {
      if (bRefresh) {
        this.getlist()
      }
      this.init.dialog.showDialog = false
      this.init.dialog.Id = null
      this.init.dialog.title = ''
    },
    //  查询按钮
    btnlistQuery: function() {
      this.drawPie('pieReport')
    },
    drawPie(id) {
      this.charts = echarts.init(document.getElementById(id))
      this.charts.setOption({
        tooltip: {
          trigger: 'item',
          formatter: '{a}<br/>{b}:{c} ({d}%)'
        },
        legend: {
          bottom: 10,
          left: 'center',
          data: this.opinion
        },
        series: [
          {
            name: '状态',
            type: 'pie',
            radius: '65%',
            center: ['50%', '50%'],
            itemStyle: {
              emphasis: {
                shadowBlur: 10,
                shadowOffsetX: 0,
                shadowColor: 'rgba(0, 0, 0, 0.5)'
              },
              color: function(params) {
                // 自定义颜色
                var colorList = ['#1ab394', '#79d2c0']
                return colorList[params.dataIndex]
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
