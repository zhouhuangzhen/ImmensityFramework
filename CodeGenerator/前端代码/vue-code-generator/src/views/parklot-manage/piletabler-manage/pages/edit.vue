
<template>
  <el-dialog
    :visible.sync="options.showDialog"
    :title="options.title"
    :width="options.width"
    :destroy-on-close="true"
    :show-close="false"
    :close-on-click-modal="false"
    @open="initData"
  >
    <el-form ref="form" :model="form" label-width="80px" size="mini" :rules="rules" :disabled="form.disabled">
      <el-form-item label prop="Guid" style="display:none;">
        <el-input v-model="form.Guid" type="hidden" />
      </el-form-item>
      <el-form-item label="登记日期" prop="RegisterDate">
        <el-date-picker
          v-model="form.RegisterDate"
          type="date"
          format="yyyy-MM-dd"
          value-format="yyyy-MM-dd"
          placeholder="请选择登记日期"
          style="width:210px;"
        />
      </el-form-item>
      <el-form-item label="充电站名" prop="CStationName">
        <el-select
          ref="CStationName"
          v-model="form.CStationName"
          placeholder="请选择"
          style="width:210px;"
          @change="changePile"
        >
          <el-option
            v-for="item in init.stations"
            :key="item.key"
            :label="item.label"
            :value="item.value"
          />
        </el-select>
      </el-form-item>
      <el-form-item label="隶属部门" prop="Department">
        <el-select
          v-model="form.Department"
          placeholder="请选择"
          style="width:210px;"
          :disabled="true"
        >
          <el-option key="empty" label="--请选择--" value />
          <el-option key="suzhoutraffice" label="苏州公共交通有限公司" value="苏州公共交通有限公司" />
          <el-option key="xiangchengtraffice" label="苏州相城区公共交通有限公司" value="苏州相城区公共交通有限公司" />
          <el-option key="wuzhongbus" label="苏州市吴中区公共汽车有限公司" value="苏州市吴中区公共汽车有限公司" />
        </el-select>
      </el-form-item>
      <el-form-item label="员工工号" prop="EmpNo">
        <el-input v-model="form.EmpNo" placeholder="请输入员工工号" style="width:210px;" />
      </el-form-item>
      <el-form-item label="员工姓名" prop="EName">
        <el-input v-model="form.EName" placeholder="请输入员工姓名" style="width:210px;" />
      </el-form-item>

      <el-form-item size="large" style="text-align:right;">
        <el-button type="primary" @click="btnSave">保存</el-button>
        <el-button @click="btnClose">关闭</el-button>
      </el-form-item>
    </el-form>
  </el-dialog>
</template>

<script>
import communicate from '@/utils/communicate.js'
import generate from '@/utils/generatehelper.js'
import convert from '@/utils/convert.js'

export default {
  name: 'Edit',
  props: {
    options: Object
  },
  data: function() {
    return {
      rules: {
        RegisterDate: [
          { required: true, message: '登记日期不能为空', trigger: 'blur' }
        ],
        CStationName: [
          { required: true, message: '充电站名不能为空', trigger: 'change' }
        ],
        EmpNo: [
          { required: true, message: '员工工号不能为空', trigger: 'blur' }
        ],
        EName: [
          { required: true, message: '员工姓名不能为空', trigger: 'blur' }
        ]
      },
      init: {
        disabled: false,
        stations: []
      },
      form: {
        Guid: this.options.id,
        CStationName: null,
        Department: null,
        RegisterDate: null,
        EmpNo: '',
        EName: ''
      }
    }
  },
  methods: {
    //  下拉框值变化
    changePile: function(selection) {
      if (this.options.piles) {
        const sites = this.options.piles
        if (Array.isArray(sites)) {
          sites.map(item => {
            if (
              selection ==
              item.PileNum + '&' + item.PowerRatio + '&' + item.Department
            ) {
              this.form.Department = item.Department
            }
          })
        } else {
          this.form.Department = sites.Department
        }
      }
    },
    //  获取充电桩下拉菜单信息
    getPileStations: function() {
      this.init.stations = []
      if (this.options.piles) {
        const sites = this.helpers.convert.object2array(this.options.piles)
        sites.map(item => {
          this.init.stations.push({
            label: item.PowerRatio + '--' + item.CStationName,
            value:
                item.PileNum + '&' + item.PowerRatio + '&' + item.Department,
            key: item.Guid
          })
        })
      }
      this.init.stations.unshift({
        label: '--请选择--',
        value: '',
        key: 'empty'
      })
    },
    btnSave: function() {
      const vm = this
      vm.form.CStationName = this.$refs.CStationName.selectedLabel
      this.$refs.form.validate(valid => {
        if (valid) {
          if (this.options.id) {
            vm.form.Guid = this.options.id
            communicate
              .SetData('MRWorkerInfo', vm.helpers.convert.null2empty(vm.form))
              .then(data => {
                if (data == 0) {
                  this.$emit('refresh', true)
                }
              })
              .catch(error => {
                console.log('异常 : ', error)
              })
          } else {
            vm.form.Guid = generate.uuid()
            communicate
              .SetData('MRWorkerInfo', vm.helpers.convert.null2empty(vm.form))
              .then(data => {
                if (data == 0) {
                  this.$emit('refresh', true)
                }
              })
              .catch(error => {
                console.log('异常 : ', error)
              })
          }
        }
      })
    },
    btnClose: function() {
      this.resetData()
      this.$emit('refresh', false)
    },
    resetData: function() {
      this.form.CStationName = null
      this.form.Department = null
      this.form.RegisterDate = null
      this.form.EmpNo = null
      this.form.EName = null
    },
    initData: function() {
      this.resetData()
      //  获取所有stations
      this.getPileStations()
      //  获取初始化数据
      communicate
        .GetData('MRWorkerInfo', this.options.id, { Guid: this.options.id })
        .then(data => {
          if (data.Guid) {
            this.form.CStationName = data.CStationName
            if (data.RegisterDate && data.RegisterDate.length > 0) {
              this.form.RegisterDate = this.parseTime(
                new Date(data.RegisterDate),
                '{y}-{m}-{d}'
              )
            }
            this.form.Department = data.Department
            this.form.EmpNo = data.EmpNo
            this.form.EName = data.EName
          }
        })
    }
  }
}
</script>

<style scoped>
</style>
