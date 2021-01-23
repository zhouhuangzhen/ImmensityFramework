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
      <el-form-item label="线路番号">
        <el-select v-model="listQuery.LName" style="width:210px;" @change="linechange">
          <el-option
            v-for="item in init.search.lineNumber"
            :key="item.key"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="线路方向">
        <el-select v-model="listQuery.lDirection" style="width:210px;">
          <el-option
            v-for="item in init.search.lineDirect"
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
      <el-tabs v-model="listQuery.SortMethod" type="card" @tab-click="handleClick">
        <el-tab-pane label="分公司" name="CompName" />
        <el-tab-pane label="线路" name="LName" />
      </el-tabs>
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
          v-if="listQuery.SortMethod =='CompName'"
          prop="CompName"
          :label="$t('detail.company')"
          :resizable="true"
          align="center"
        />
        <el-table-column v-if="listQuery.SortMethod !='CompName'" prop="LName" width="180" label="线路" :resizable="true" align="center" />
        <el-table-column v-if="listQuery.SortMethod !='CompName'" prop="lDirection" label="方向" :resizable="true" align="center" />
        <el-table-column prop="Num" width="180" label="数量" :resizable="true" align="center" />
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
import communicate from '@/utils/communicate.js'
import { baseinfo } from '@/utils/infohelper.js'
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
        LName: '',
        LGUID: '',
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
          lineDirect: [
            {
              key: 'empty',
              value: '',
              label: '--请选择--'
            }
          ],
          corp: [],
          line: [],
          shift: []
        },
        table: {
          height: window.innerHeight - 350,
          key: 'table',
          emptytext: '暂无数据',
          datatype: 'QueryDailyOverSpeedSummary',
          url: 'http://localhost:8016/User/GetList',
          listnode: 'Data',
          type: 'post'
        }
      },
      //    表格数据
      data: []
    }
  },
  mounted: function() {
    //  初始化线路编号下拉框的数据
    // return
    // userinfo.getLines().then(data => {
    //   const lines = data.Line
    //   const all = []
    //   for (let index = 0; index < lines.length; index++) {
    //     const item = lines[index]
    //     if (all.indexOf(item.LName) < 0) {
    //       all.push(item.LName)
    //       this.init.search.lineNumber.push({
    //         key: item.LName,
    //         value: item.LName,
    //         label: item.LName
    //       })
    //     }
    //   }
    //   this.init.search.unshift({
    //     key: 'all',
    //     value: "'" + all.join("','") + "'",
    //     label: '全部'
    //   })
    // })
    //  初始化表格数据
    this.getlist()
  },
  methods: {
    linechange: function(changeValue) {
      var vm = this
      if (!changeValue) {
        vm.init.search.lineDirect = [
          {
            key: 'empty',
            value: '',
            label: '--请选择--'
          }
        ]
      } else {
        baseinfo.getLineinfoByName(changeValue).then(data => {
          vm.init.search.lineDirect = [
            {
              key: 'empty',
              value: '',
              label: '--请选择--'
            }
          ]
          if (data && data.LineInfo) {
            if (Array.isArray(data.LineInfo)) {
              for (let index = 0; index < data.LineInfo.length; index++) {
                const line = data.LineInfo[index]
                vm.init.search.lineDirect.push({
                  key: line.LGUID,
                  value: line.LDirection,
                  label: line.LDirection
                })
              }
            } else {
              vm.init.search.lineDirect.push({
                key: data.LineInfo.LGUID,
                value: data.LineInfo.LDirection,
                label: data.LineInfo.LDirection
              })
            }
          }
        })
      }
    },
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
            excelhelper.export('接龙统计', headers, columns, data)
          } else {
            vm.msgError('无数据')
          }
          vm.unloading(myload)
        })
    },
    //  切换按钮
    handleClick: function() {
      this.btnlistQuery()
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
