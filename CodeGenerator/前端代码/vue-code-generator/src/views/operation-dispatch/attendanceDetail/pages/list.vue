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
    </el-row>
    <el-row>
      <el-col :span="2" class="label" v-text="$t('search.drivername')" />
      <el-col :span="4">
        <el-input v-model="listQuery.drivername" />
      </el-col>
      <el-col :span="2" class="label" v-text="$t('search.driveremp')" />
      <el-col :span="4">
        <el-input v-model="listQuery.driveremp" />
      </el-col>
      <el-col :span="2" class="label" v-text="$t('search.atttype')" />
      <el-col :span="4">
        <el-select v-model="listQuery.atttype" :placeholder="$t('ddllist.pleaseselect')">
          <el-option key="all" :label="$t('ddllist.all')" value />
          <el-option key="checkin" :label="$t('ddllist.checkin')" value="1" />
          <el-option key="checkout" :label="$t('ddllist.checkout')" value="2" />
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
                <el-table-column :label="$t('detail.index')" align="center" width="70">
          <template scope="scope"><span>{{ scope.$index+(listQuery.CurrentPage - 1) * listQuery.PageSize + 1 }} </span></template>
        </el-table-column>
        <el-table-column prop="name" width="180" :label="$t('detail.date')" :resizable="true" align="center" />
        <el-table-column prop="name" width="180" :label="$t('detail.driveremp')" :resizable="true" align="center" />
        <el-table-column prop="name" width="180" :label="$t('detail.drivername')" :resizable="true" align="center" />
        <el-table-column prop="name" width="180" label="实际签到时间" :resizable="true" align="center" />
        <el-table-column prop="name" width="180" :label="$t('detail.status')" :resizable="true" align="center" />
        <el-table-column prop="remark" :label="$t('detail.remark')" :resizable="true" align="center" />
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
.el-row {
  margin: 5px auto;
}

.label {
  text-align: center;
  line-height: 36px;
  font-size: 14px;
}
</style>
