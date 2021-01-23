<template>
  <el-form ref="form" :model="form" label-width="80px" size="mini" :disabled="form.disabled">
    <el-form-item label prop="Id" style="display:none;">
      <el-input v-model="form.Id" type="hidden" />
    </el-form-item>
    <el-form-item label="内容" prop="Content">
      <el-input v-model="form.Content" type="textarea" :rows="5" placeholder="请输入内容" />
    </el-form-item>
    <el-form-item label="类型" prop="Language">
      <el-select v-model="form.Language" placeholder="请选择">
        <el-option key="empty" label="--请选择--" value />
        <el-option key="user" label="人员例检" value="1" />
        <el-option key="car" label="车辆例检" value="2" />
      </el-select>
    </el-form-item>
    <el-form-item label="顺序" prop="Order">
      <el-input-number v-model="form.Order" :min="1" :max="10" label="顺序" @change="handleChange" />
    </el-form-item>
    <el-form-item label="备注" prop="Remark">
      <el-input v-model="form.Remark" type="textarea" :rows="5" placeholder="请输入备注" />
    </el-form-item>
    <el-form-item size="large" style="text-align:right;" :v-show="!form.disabled">
      <el-button type="primary" @click="btnSave">保存</el-button>
      <el-button @click="btnClose">关闭</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
import request from '@/utils/action.js'

export default {
  name: 'Edit',
  props: {
    options: Object
  },
  data: function() {
    return {
      form: {
        Id: this.options.Id,
        disabled: false,
        Conent: null,
        Type: null,
        Order: null,
        Remark: null
      }
    }
  },
  methods: {
    btnSave: function() {
      const vm = this
      request({
        url: 'http://localhost:8016/User/Edit?key=' + vm.form.Id,
        type: 'post',
        data: vm.form,
        success: function(data) {
          if (data.success) {
            vm.$refs.form.resetFields()
            vm.$emit('refresh', true)
          }
        }
      })
    },
    btnClose: function() {
      this.$emit('refresh', false)
    },
    initData: function() {
      //    信息初始化
      if (this.options.Id) {
        const vm = this
        request({
          url: 'http://localhost:8016/User/GetUser?id=' + vm.options.Id,
          type: 'get',
          success: function(data) {
            if (data.success) {
              data.data.Type = data.data.Type.toString()
              vm.form = data.data
              vm.form.disabled = vm.options.type === 'detail'
            }
          },
          error: function(err) {
            console.log(err)
          }
        })
      }
    }
  }
}
</script>

<style scoped>
</style>
