<template>
  <div class="app-container">
    <!--  查询条件  -->
    <el-form ref="queryForm" :model="listQuery" :inline="true">
      <el-form-item label="日期">
        <el-date-picker v-model="listQuery.Date" type="date" value-format="yyyy-MM-dd" />
      </el-form-item>
      <el-form-item label="工号">
        <el-input v-model="listQuery.EmpNo" placeholder="请输入工号" style="width:200px;" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" icon="el-icon-search" size="mini" @click="btnlistQuery">搜索</el-button>
      </el-form-item>
    </el-form>
    <!--  图标  -->
    <div class="area">
      <el-steps direction="vertical">
        <el-timeline>
          <el-timeline-item>
            <el-card>
              <h4>签到</h4>
              <p class="timeline-content">{{ checkIn }}</p>
              <h4>登录</h4>
              <p class="timeline-content">{{ signIn }}</p>
            </el-card>
          </el-timeline-item>
        </el-timeline>
      </el-steps>
      <el-steps direction="vertical">
        <el-timeline>
          <el-timeline-item
            v-for="item in data"
            :key="item"
            :timestamp="item.PlanTime"
            placement="top"
            size="normal"
          >
            <el-card>
              <h4>调度指令下发</h4>
              <p class="timeline-content">{{ item.SendTime }}</p>
              <h4>驾驶员应答</h4>
              <p class="timeline-content">{{ item.RecTime }}</p>
            </el-card>
          </el-timeline-item>
        </el-timeline>
      </el-steps>
      <el-steps direction="vertical">
        <el-timeline>
          <el-timeline-item>
            <el-card>
              <h4>注销</h4>
              <p class="timeline-content">{{ signOut }}</p>
              <h4>签退</h4>
              <p class="timeline-content">{{ checkOut }}</p>
            </el-card>
          </el-timeline-item>
        </el-timeline>
      </el-steps>
    </div>
  </div>
</template>

<script>
import communicate from '@/utils/communicate.js'

export default {
  name: 'List',
  data: function() {
    return {
      // 合计数量
      total: 0,
      //  查询绑定参数
      listQuery: {
        CurrentPage: 1,
        PageSize: 10,
        SortMethod: 'fPlanTime'
      },
      pickerOptions: {
        disabledDate(time) {
          return time.getTime() > Date.now() - 8.64e6
        }
      },
      // 初始化参数
      init: {
        table: {
          height: window.innerHeight - 350,
          key: 'table',
          emptytext: '暂无数据',
          datatype: 'QueryDriverExecute',
          datatypeAtt: 'QueryDriverAttendance',
          url: 'http://localhost:8016/User/GetList',
          listnode: 'Data',
          type: 'post'
        }
      },
      //    表格数据
      data: [
        { PlanTime: '', SendTime: '', RecTime: '' }, { PlanTime: '', SendTime: '', RecTime: '' }
      ],
      signIn: '',
      checkIn: '',
      checkOut: '',
      signOut: ''
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
      if (!vm.listQuery.Date) {
        this.$msgError('请选择日期！')
        return
      }
      if (!vm.listQuery.EmpNo) {
        vm.$message.error('请选择工号！')
        return
      }
      const myload = vm.inloading('查询中，请稍后......')
      communicate
        .TransformData(vm.init.table.datatype, vm.listQuery, 'busCompany', 'business')
        .then(response => {
          vm.data = []
          if (response[vm.init.table.listnode]) {
            if (Array.isArray(response[vm.init.table.listnode])) {
              vm.data = response[vm.init.table.listnode]
            } else {
              vm.data.push(response[vm.init.table.listnode])
            }
          } else {
            vm.data = [{ PlanTime: '', SendTime: '', RecTime: '' }, { PlanTime: '', SendTime: '', RecTime: '' }]
            this.$message({
              message: '暂无数据！',
              type: 'warning'
            })
          }
          vm.unloading(myload)
        })
      communicate
        .TransformData(vm.init.table.datatypeAtt, vm.listQuery, 'busCompany', 'business')
        .then(response => {
          vm.signIn = response['signIn']
          vm.checkIn = response['checkIn']
          vm.checkOut = response['checkOut']
          vm.signOut = response['signOut']
        })
    },
    //  查询按钮
    btnlistQuery: function() {
      this.listQuery.CurrentPage = 1
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
