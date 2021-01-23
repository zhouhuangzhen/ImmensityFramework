<template>
  <div class="app-container">
    <!--  查询条件  -->
    <div class="area">
      <el-form ref="queryForm" :model="query" label-width="120px">
        <el-row>
          <el-col :span="8">
            <el-form-item label="发送开始时间">
              <el-date-picker
                v-model="query.initStartTime"
                type="date"
                format="yyyy-MM-dd"
                value-format="yyyy-MM-dd"
                placeholder="请选择发送开始时间"
                style="width:210px;"
                :picker-options="pickerOptionsStart"
                @change="changeEnd"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="发送结束时间">
              <el-date-picker
                v-model="query.initEndTime"
                type="date"
                format="yyyy-MM-dd"
                value-format="yyyy-MM-dd"
                placeholder="请选择发送结束时间"
                style="width:210px;"
                :picker-options="pickerOptionsEnd"
                @change="changeStart"
              />
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="接收方类型">
              <el-select v-model="query.ReceiverType" style="width:210px;">
                <el-option key="all" label="全部" value />
                <el-option key="car" label="车载" value="0" />
                <el-option key="mobile" label="手持" value="1" />
                <el-option key="other" label="其他显示设备" value="2" />
                <el-option key="warn" label="应急预案" value="4" />
              </el-select>
            </el-form-item>
          </el-col>
        </el-row>
        <el-row>
          <el-col :span="8">
            <el-form-item label="TTS类型">
              <el-select v-model="query.NoticeType" style="width:210px;">
                <el-option key="all" label="全部" value />
                <el-option key="is" label="是TTS" value="0" />
                <el-option key="not" label="否TTS" value="1" />
                <el-option key="leave" label="请假" value="2" />
                <el-option key="work" label="通知上班时间和取车地址" value="3" />
                <el-option key="repair" label="车辆报修" value="4" />
                <el-option key="getcar" label="通知取车" value="5" />
                <el-option key="logout" label="通知签退" value="6" />
                <el-option key="stop" label="车辆停运" value="7" />
                <el-option key="warncheck" label="应急预案审核" value="17" />
                <el-option key="warnstart" label="启用应急预案" value="18" />
              </el-select>
            </el-form-item>
          </el-col>
          <el-col :span="8">
            <el-form-item label="发送状态">
              <el-select v-model="query.Status" style="width:210px;">
                <el-option key="all" label="全部" value />
                <el-option key="is" label="同意" value="0" />
                <el-option key="not" label="不同意" value="1" />
                <el-option key="unsure" label="未确认" value="-1" />
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
      <el-button type="success" size="small" class="el-button--export" @click="btnExport">
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
        <el-table-column label="接收方类型" width="100" property="ReceiverType" align="center" :formatter="formatter" />
        <el-table-column label="接收方信息" width="150" property="Receiver" align="center" />
        <el-table-column label="TTS类型" width="100" property="TTSType" align="center" :formatter="formatter" />
        <el-table-column label="发送时间" width="200" property="SendTime" align="center" />
        <el-table-column label="发送状态" width="120" property="Status" align="center" :formatter="formatter" />
        <el-table-column label="发送内容" property="Content" align="center" :show-overflow-tooltip="true" />
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
      pickerOptionsStart: {},
      pickerOptionsEnd: {},
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
        SortMethod: 'SendTime',
        OrderMethod: 'desc',
        initStartTime: '2020-02-01',
        initEndTime: '2020-02-01',
        StartTime: null,
        EndTime: null,
        ReceiverType: null,
        NoticeType: null,
        Status: null
      },
      //    初始化参数
      init: {
        //  查询条件初始化参数
        search: {},
        table: {
          height: window.innerHeight - 366,
          key: 'table',
          emptytext: '暂无数据',
          datatype: 'NoticeInfoList',
          listnode: 'Notice'
        },
        dialog: {
          width: '50%',
          showDialog: false,
          title: '',
          type: 'detail',
          Id: null
        },
        sort: {
          SortMethod: 'LUTC',
          OrderMethod: 'desc'
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
    //  this.getlist()
    this.initData()
  },
  methods: {
    changeStart() {
      this.pickerOptionsStart = Object.assign({}, this.pickerOptionsStart, {
        disabledDate: (time) => {
          if (!this.query.initEndTime) return false
          return time.getTime() > new Date(this.query.initEndTime).getTime()
        }
      })
    },
    changeEnd() {
      this.pickerOptionsEnd = Object.assign({}, this.pickerOptionsEnd, {
        disabledDate: (time) => {
          if (!this.query.initStartTime) return false
          return time.getTime() < new Date(this.query.initStartTime).getTime()
        }
      })
    },
    initData() {
      this.query.initStartTime = this.helpers.date.fromToday(-1)
      this.query.initEndTime = this.helpers.date.fromToday(-1)
    },
    //  列format事件
    formatter: function(row, column, cellValue, index) {
      let value = null
      switch (column.label) {
        case '接收方类型': {
          switch (cellValue) {
            case '0': {
              value = '车载'
              break
            }
            case '1': {
              value = '手持'
              break
            }
            case '2': {
              value = '其他显示设备'
              break
            }
          }
          break
        }
        case 'TTS类型': {
          switch (cellValue) {
            case '0': {
              value = '是TTS'
              break
            }
            case '1': {
              value = '否TTS'
              break
            }
            case '2': {
              value = '请假'
              break
            }
            case '3': {
              value = '通知上班时间和取车地址'
              break
            }
            case '4': {
              value = '车辆报修'
              break
            }
            case '5': {
              value = '通知取车'
              break
            } case '6': {
              value = '通知签退'
              break
            } case '7': {
              value = '车辆停运'
              break
            }
          }
          break
        }
        case '发送状态': {
          switch (cellValue) {
            case '0': {
              value = '同意'
              break
            }
            case '1': {
              value = '不同意'
              break
            }
            case '-1': {
              value = '未确认'
              break
            }
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
      vm.loading = true
      //  接口对象处理
      if (!vm.query.ReceiverType) {
        vm.$delete(vm.query, 'ReceiverType')
      }
      if (!vm.query.NoticeType) {
        vm.$delete(vm.query, 'NoticeType')
      }
      if (!vm.query.Status) {
        vm.$delete(vm.query, 'Status')
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
      if (
        !this.query.initStartTime ||
        this.query.initStartTime.length <= 0 ||
        !this.query.initEndTime ||
        this.query.initEndTime.length <= 0) {
        this.msgError('开始时间和结束时间不能为空')
        return
      }
      this.pager.page = 1
      this.getlist()
    },
    //  导出按钮
    btnExport: function() {
      var vm = this

      //  接口对象处理
      if (!vm.query.DBusCard) {
        vm.$delete(vm.query, 'DBusCard')
      }
      if (!vm.query.BusNo) {
        vm.$delete(vm.query, 'BusNo')
      }
      if (!vm.query.EName) {
        vm.$delete(vm.query, 'EName')
      }
      if (!vm.query.EmpNo) {
        vm.$delete(vm.query, 'EmpNo')
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
            const exist = ['操作']
            const data = convert.object2array(response[vm.init.table.listnode])
            this.$refs.table.columns.map(item => {
              if (item.property && (item.type == 'default' || item.type == 'text') && columns.indexOf(item.property) < 0 && exist.indexOf(item.label) < 0) {
                columns.push(item.property)
                headers.push(item.label)
              }
            })

            data.map(item => {
              // 接收方类型格式化
              switch (item.ReceiverType) {
                case '0': {
                  item.ReceiverType = '车载'
                  break
                }
                case '1': {
                  item.ReceiverType = '手持'
                  break
                }
                case '2': {
                  item.ReceiverType = '其他显示设备'
                  break
                }
              }
              // TTS类型格式化
              switch (item.TTSType) {
                case '0': {
                  item.TTSType = '是TTS'
                  break
                }
                case '1': {
                  item.TTSType = '否TTS'
                  break
                }
                case '2': {
                  item.TTSType = '请假'
                  break
                }
                case '3': {
                  item.TTSType = '通知上班时间和取车地址'
                  break
                }
                case '4': {
                  item.TTSType = '车辆报修'
                  break
                }
                case '5': {
                  item.TTSType = '通知取车'
                  break
                } case '6': {
                  item.TTSType = '通知签退'
                  break
                } case '7': {
                  item.TTSType = '车辆停运'
                  break
                }
              }
              // 发送状态格式化
              switch (item.Status) {
                case '0': {
                  item.Status = '同意'
                  break
                }
                case '1': {
                  item.Status = '不同意'
                  break
                }
                case '-1': {
                  item.Status = '未确认'
                  break
                }
              }
            })
            excelhelper.export('车辆信息发送表', headers, columns, data)
          } else {
            vm.msgError('无数据')
          }
          vm.unloading(myload)
        }).catch(error => {
          console.log(error)
          vm.unloading(myload)
          this.msgError(this.errorMessage['Commmons']['1'])
        })
    },
    btnAdd() {},
    btnImport() {},
    btnTemplate() {}
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
