�� �� �V �]�c �uFT �*N$�P[ �`O1rb �Q�� �Y�N��/f�l �v}Y �aW�N �a�k �a<� �Q�N ��?V?V �U�U �U�U}Y �mˆ ��`O�4V ��To� ��KN�_{ ���` ���| ���N ��0R ���R �Q ��W �Sl �[Nw� ��CQ�[ ���e%f ��>�l ���� �Q�S �Qeg�Q�l �QWl �Q�s �Q;�@� �W�S �W�` �W�`�V\ ��� �N'` �}v<w �T �T�T �T�T�T �TLk �V�e �b �� N7h ��/fWY� ��/fb�v �R_ �R_ NN �R_�N �R_b ��I� �b�~S                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    pe="info" size="small" class="el-button--export" @click="btnExport">
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
        <el-table-column width="180" label="操作" property="GUID" align="center">
          <template slot-scope="scope">
            <el-link type="primary" @click="queryChange(scope.row)">查看变动</el-link>
          </template>
        </el-table-column>
        <el-table-column width="100" label="路牌编号" property="LineNO" align="center" />
        <el-table-column width="200" label="调整发车时间" property="AdjustTime" align="center" />
        <el-table-column width="200" label="调整车牌号" property="AdjustDBusCard" align="center" />
        <el-table-column width="150" label="调整驾驶员姓名" property="AdjustEName" align="center" />
        <el-table-column width="100" label="线路名称" property="LName" align="center" />
        <el-table-column
          width="100"
          label="指令"
          property="AlreadySendDisp"
          align="center"
          :formatter="formatter"
        />
        <el-table-column
          width="100"
          label="班次类型"
          property="BSType"
          align="center"
          :formatter="formatter"
        />
        <el-table-column label="调度类型" property="DType" align="center" :formatter="formatter" />
      </el-table>

      <pagination
        v-show="total>0"
        :total="total"
        :page.sync="pager.page"
        :limit.sync="pager.limit"
        @pagination="getlist"
      />
    </div>
    <Edit ref="edit" :options="init.dialog" />
  </div>
</template>

<script>
import communicate from '@/utils/communicate.js'
import excelhelper from '@/utils/excelhelper.js'
import convert from '@/utils/convert.js'
import Edit from './edit.vue'

export default {
  name: 'List',
  components: { Edit },
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
        SortMethod: 'LName',
        OrderMethod: 'desc',
        initBeginDate: '2020-02-01',
        initEndDate: '2020-02-01',
        BeginDate: null,
        EndDate: null,
        LName: null,
        DBusCard: null,
        AdjustType: null,
        BSType: null
      },
      //    初始化参数
      init: {
        //  查询条件初始化参数
        search: {},
        table: {
          height: window.innerHeight - 366,
          key: 'table',
          emptytext: '暂无数据',
          datatype: 'BusShiftInfoDetailList',
          listnode: 'BusShiftDetailInfo'
        },
        dialog: {
          width: '50%',
          showDialog: false,
          title: '',
          type: 'detail',
          id: null
        },
        sort: {
          SortMethod: 'LName',
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
    this.getlist()
  },
  methods: {
    //  列format事件
    formatter: function(row, column, cellValue, index) {
      let value = ''
      switch (column.label) {
        case '调度类型': {
          if (!row.DBusCard || row.DBusCard.length <= 0) {
            value = '新增'
          } else {
            value = '变动'
          }
          break
        }
        case '班次类型': {
          switch (cellValue) {
            case '0': {
              value = '正常'
              break
            }
            case '1': {
              value = '首班'
              break
            }
            case '2': {
              value = '临时'
              break
            }
            case '3': {
              value = '删除'
              break
            }
            case '4': {
              value = '停运'
              break
            } case '5': {
              value = '预处理'
              break
            } case '6': {
              value = '取消'
              break
            } case '7': {
              value = '待发'
              break
            } case '8': {
              value = '已发'
              break
            } case '9': {
              value = '已完成'
              break
            } case '10': {
              value = '中途停运'
              break
            } default: {
              value = '其他'
              break
            }
          }
          break
        }
        case '指令': {
          switch (cellValue) {
            case '0': {
              value = '未发送'
              break
            }
            case '1': {
              value = '车已确认'
              break
            }
            case '2': {
              value = '已下发'
              break
            }
            case '3': {
              value = '人已确认'
              break
            }
            case '4': {
              value = '全部确认'
              break
            }
            case '-1': {
              value = '超时未发送'
              break
            } default: {
              value = '其他'
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
      if (!vm.query.initBeginDate) {
        vm.$delete(vm.query, 'BeginDate')
      } else {
        vm.query.BeginDate = vm.query.initBeginDate + ' 00:00:00.000'
      }
      if (!vm.query.initEndDate) {
        vm.$delete(vm.query, 'EndDate')
      } else {
        vm.query.EndDate = vm.query.initEndDate + ' 23:59:59.999'
      }

      if (pager === undefined) {
        vm.query.CurrentPage = 0
      } else {
        vm.query.CurrentPage = pager.page - 1
        vm.query.PageSize = pager.limit
        vm.pager = pager
      }

      communicate
        .TransformData(vm.init.table.datatype, vm.helpers.convert.null2empty(vm.query))
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
    //  查看变动
    queryChange(row) {
      console.log(row)
      this.init.dialog.showDialog = true
      this.init.dialog.id = row.GUID
      this.init.dialog.title = '查看变动'
    },
    //  查询按钮
    btnQuery: function() {
      this.pager.page = 1
      this.getlist()
    },
    //  导出按钮
    btnExport: function() {
      var vm = this
      //  接口对象处理
      if (!vm.query.initBeginDate) {
        vm.$delete(vm.query, 'BeginDate')
      } else {
        vm.query.BeginDate = vm.query.initBeginDate + ' 00:00:00.000'
      }
      if (!vm.query.initEndDate) {
        vm.$delete(vm.query, 'EndDate')
      } else {
        vm.query.EndDate = vm.query.initEndDate + ' 23:59:59.999'
      }

      const exportquery = {}
      Object.assign(exportquery, vm.helpers.convert.null2empty(vm.query))
      exportquery.CurrentPage = 0
      exportquery.PageSize = 999999

      const myload = vm.inloading('导出中，请稍后......')
      communicate
        .TransformData(vm.init.table.datatype, exportquery)
        .then(response => {
          if (response[vm.init.table.listnode]) {
            const columns = []
            const headers = []
            const exist = ['操作', '调度类型']
            const data = convert.object2array(response[vm.init.table.listnode])
            console.log(this.$refs.table.columns)
            this.$refs.table.columns.map(item => {
              if (item.property && (item.type == 'default' || item.type == 'text') && columns.indexOf(item.property) < 0 && exist.indexOf(item.label) < 0) {
                columns.push(item.property)
                headers.push(item.label)
              }
            })
            data.map(item => {
              switch (item.BSType) {
                case '0': {
                  item.BSType = '正常'
                  break
                }
                case '1': {
                  item.BSType = '首班'
                  break
                }
                case '2': {
                  item.BSType = '临时'
                  break
                }
                case '3': {
                  item.BSType = '删除'
                  break
                }
                case '4': {
                  item.BSType = '停运'
                  break
                } case '5': {
                  item.BSType = '预处理'
                  break
                } case '6': {
                  item.BSType = '取消'
                  break
                } case '7': {
                  item.BSType = '待发'
                  break
                } case '8': {
                  item.BSType = '已发'
                  break
                } case '9': {
                  item.BSType = '已完成'
                  break
                } case '10': {
                  item.BSType = '中途停运'
                  break
                } default: {
                  item.BSType = '其他'
                  break
                }
              }
              switch (item.AlreadySendDisp) {
                case '0': {
                  item.AlreadySendDisp = '未发送'
                  break
                }
                case '1': {
                  item.AlreadySendDisp = '车已确认'
                  break
                }
                case '2': {
                  item.AlreadySendDisp = '已下发'
                  break
                }
                case '3': {
                  item.AlreadySendDisp = '人已确认'
                  break
                }
                case '4': {
                  item.AlreadySendDisp = '全部确认'
                  break
                }
                case '-1': {
                  item.AlreadySendDisp = '超时未发送'
                  break
                } default: {
                  item.AlreadySendDisp = '其他'
                  break
                }
              }
            })
            excelhelper.export('调整日报表', headers, columns, data)
          } else {
            vm.msgError('无数据')
          }
          vm.unloading(myload)
        }).catch(error => {
          console.log(error)
          this.msgError(this.errorMessage['Commmons']['1'])
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
.label {
  text-align: center;
  line-height: 36px;
  font-size: 14px;
}
</style>
