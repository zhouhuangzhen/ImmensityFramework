/* eslint-disable vue/this-in-template */
<template>
  <div class="app-container">
    <!--  查询条件  -->
    <el-row>
      <el-col :span="2" class="label" v-text="$t('search.drivername')" />
      <el-col :span="4">
        <el-input v-model="listQuery.drivername" />
      </el-col>
      <el-col :span="2" class="label" v-text="$t('search.driveremp')" />
      <el-col :span="4">
        <el-input v-model="listQuery.driveremp" />
      </el-col>
      <el-col :span="2" class="label" v-text="$t('search.deptname')" />
      <el-col :span="4">
        <el-input v-model="listQuery.deptname" />
      </el-col>
      <el-col :span="2" class="label" v-text="$t('search.month')" />
      <el-col :span="4">
        <el-date-picker v-model="listQuery.month" type="month" format="yyyy 年 MM 月" value-format="yyyyMM" />
      </el-col>
    </el-row>
    <!--  按钮区域  -->
    <div style="text-align:right;" class="area">
      <el-button type="primary" size="small" @click="btnlistQuery">
        <i class="el-icon-upload" />
        查询
      </el-button>
    </div>
    <!--  数据表格  -->
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
        <el-table-column prop="name" width="180" label="所属部门" :resizable="true" align="center" />
        <el-table-column prop="name" width="180" label="月份" :resizable="true" align="center" />
        <el-table-column prop="name" width="180" label="工号" :resizable="true" align="center" />
        <el-table-column prop="name" width="180" label="姓名" :resizable="true" align="center" />
        <el-table-column label="日期" align="center">
          <el-table-column prop="name" width="50" label="1" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="2" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="3" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="4" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="5" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="6" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="7" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="8" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="9" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="10" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="11" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="12" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="13" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="14" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="15" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="16" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="17" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="18" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="19" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="20" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="21" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="22" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="23" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="24" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="25" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="26" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="27" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="28" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="29" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="30" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="31" :resizable="true" align="center" />
        </el-table-column>
        <el-table-column label="累计上班天数" align="center">
          <el-table-column prop="name" width="50" label="早" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="中" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="二" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="吃" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="夜" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="机" :resizable="true" align="center" />
          <el-table-column prop="name" width="50" label="休" :resizable="true" align="center" />
        </el-table-column>
        <el-table-column prop="name" width="180" label="合计" :resizable="true" align="center" />
        <el-table-column prop="name" width="180" label="合计工作小时" :resizable="true" align="center" />
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
    </div>
  </div>
</template>

<script>
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
          height: window.innerHeight - 286,
          key: 'table',
          emptytext: '暂无数据',
          url: 'http://localhost:8016/User/GetList',
          type: 'post'
        }
      },
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
    // init.table.height =
    //   window.innerHeight - $refs.table.$el.offsetTop - 50;
    //  console.log(init.table.height);
    this.getlist()
  },
  methods: {
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
      vm.listQuery.pageIndex = vm.pager.index
      vm.listQuery.pageSize = vm.pager.size
      request({
        url: vm.init.table.url,
        type: vm.init.table.type,
        data: vm.listQuery,
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
      this.getlist()
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
