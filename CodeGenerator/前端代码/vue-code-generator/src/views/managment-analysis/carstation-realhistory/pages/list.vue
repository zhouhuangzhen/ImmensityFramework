<template>
  <div class="app-container">
    <!--  查询条件  -->
    <div class="area">
      <el-form ref="queryForm" :model="query" label-width="120px">
        <el-row>
          <el-col :span="6">
            <el-form-item label="日期">
              <el-date-picker
                v-model="query.Time"
                type="date"
                format="yyyy-MM-dd"
                value-format="yyyy-MM-dd"
                placeholder="请选择时间"
                style="width:210px;"
              />
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="线路">
              <el-select v-model="query.LName" style="width:210px;">
                <el-option
                  v-for="item in init.search.lines"
                  :key="item.key"
                  :label="item.label"
                  :value="item.value"
                />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="方向">
              <el-select v-model="query.LGUID" style="width:210px;">
                <el-option
                  v-for="item in init.search.directions"
                  :key="item.key"
                  :label="item.label"
                  :value="item.value"
                />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="6">
            <el-form-item label="车号">
              <el-input v-model="query.BusNo" placeholder="请输入车牌号" style="width:210px;" />
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
        <el-table-column width="180" label="车载设备标识" property="DGUID" align="center" />
        <el-table-column width="100" label="企业自编号" property="BusNo" align="center" />
        <el-table-column width="200" label="车牌号" property="DBusCard" align="center" />
        <el-table-column width="200" label="线路番号" property="LName" align="center" />
        <el-table-column width="100" label="线路方向" property="LDirection" align="center" />
        <el-table-column
          width="100"
          label="站序"
          property="SLNO"
          align="center"
          :formatter="formatter"
        />
        <el-table-column width="100" label="站点名称" property="LSName" align="center" />
        <el-table-column width="100" label="进站时间" property="DInTime" align="center" />
        <el-table-column width="100" label="出站时间" property="DOutTime" align="center" />
        <el-table-column width="100" label="车辆方向" property="LDir" align="center" />
        <el-table-column width="100" label="数据来源" property="SDevice" align="center" />
        <el-table-column label="状态描述" property="LVDesc" align="center" />
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
        SortMethod: 'DispTime',
        OrderMethod: 'asc',
        Time: null,
        StartTime: null,
        EndTime: null,
        LName: null,
        LGUID: null,
        DBusCard: null,
        BusNo: null
      },
      //    初始化参数
      init: {
        //  查询条件初始化参数
        search: {
          lines: [],
          directions: []
        },
        table: {
          height: window.innerHeight - 286,
          key: 'table',
          emptytext: '暂无数据',
          datatype: 'BSLineSitePositionList',
          listnode: 'PositionInfo'
        },
        dialog: {
          width: '50%',
          showDialog: false,
          title: '',
          type: 'detail',
          Id: null
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
  created: function(pager) {
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
    this.initData()
    this.getlist()
  },
  methods: {
    //  数据初始化
    initData() {
      this.init.search.lines = [
        {
          label: '--请选择--',
          value: '',
          key: 'empty'
        },
        {
          label: '1',
          value: 'b9434375-db2d-49c0-9561-938fa7b29071,9d090af5-c5c6-4db8-b34e-2e8af4f63216',
          key: '1'
        },
        {
          label: '100',
          value: '59b4646c-2073-4fb9-9093-cb0daf02e4a0,f16b69c8-ddef-40e1-895e-ffb6c5c9587a,fbe2afe7-aa3a-465b-947a-312bc24cdbe9,2cbf185e-20e2-49b7-b227-aae85e39e8aa',
          key: '100'
        },
        {
          label: '900',
          value: 'c887aa5e-148d-46c9-b450-0d21807a9de7,8f2ac361-f6cc-4a43-8dcb-c44249799529',
          key: '900'
        },
        {
          label: '126',
          value: 'a7124a1c-1204-43e5-b4b6-765fb336ccc0,f7c676c6-0b55-41e4-826f-807e57fed9b4',
          key: '126'
        }
      ]
      this.init.search.directions = [
        {
          label: '全部',
          value: '',
          key: 'empty'
        },
        {
          label: '苏州站北广场公交枢纽',
          value: 'b9434375-db2d-49c0-9561-938fa7b29071',
          key: 'b9434375-db2d-49c0-9561-938fa7b29071'
        },
        {
          label: '公交一路新村',
          value: '9d090af5-c5c6-4db8-b34e-2e8af4f63216',
          key: '9d090af5-c5c6-4db8-b34e-2e8af4f63216'
        }
      ]
    },
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
      if (this.$refs[this.init.table.key]) this.$refs[this.init.table.key].bodyWrapper.scrollTop = 0
      //  接口对象处理
      if (!vm.query.DBusCard) {
        vm.$delete(vm.query, 'DBusCard')
      }
      if (!vm.query.BusNo) {
        vm.$delete(vm.query, 'BusNo')
      }
      if (!vm.query.LGUID) {
        vm.$delete(vm.query, 'LGUID')
      }
      if (!vm.query.LName) {
        vm.$delete(vm.query, 'LName')
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

      if (!vm.query.DBusCard) {
        vm.$delete(vm.query, 'DBusCard')
      }
      if (!vm.query.BusNo) {
        vm.$delete(vm.query, 'BusNo')
      }
      if (!vm.query.LGUID) {
        vm.$delete(vm.query, 'LGUID')
      }
      if (!vm.query.LName) {
        vm.$delete(vm.query, 'LName')
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
              item.SignTime = this.parseTime(new Date(item.SignTime), '{y}-{m}-{d}')
              // 状态格式化
              switch (item.Status) {
                case '0': {
                  item.Status = '签到'
                  break
                } case '1': {
                  item.Status = '签退'
                  break
                } default: {
                  item.Status = ''
                  break
                }
              }
            })
            excelhelper.export('考勤信息表', headers, columns, data)
          } else {
            vm.msgError('无数据')
          }
          vm.unloading(myload)
        }).catch(error => �ڕ�.                     ����    ����    ]  �4�T3F!              D      @�  ]          ,   �     P      P  ,              p     �     �     �  %   0     �     �     0  L   �  
   �     P       �   �"     @#     0%      &      &     �&     �'     �'     (     p(     �(     �(     �(     �(  A   �,     0-     P-     �.     /     �/      0     P0  !   `2     �3     @4  *   �6     �7     8     p8     �8  	   09     @9     ;     0;     P;     @<     P<     �<     =     `=     �=     p>     `?    