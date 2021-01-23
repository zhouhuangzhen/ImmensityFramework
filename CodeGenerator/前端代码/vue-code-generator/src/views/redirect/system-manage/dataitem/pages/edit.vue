<template>
  <el-form ref="form" :model="form" label-width="100px" size="mini" :disabled="form.disabled">
    <el-form-item label prop="Id" style="display:none;">
      <el-input v-model="form.Id" type="hidden" />
    </el-form-item>
    <el-form-item label="编码" prop="Code">
      <el-input v-model="form.Code" />
    </el-form-item>
    <el-form-item label="名称" prop="Name">
      <el-input v-model="form.Name" />
    </el-form-item>
    <el-form-item label="值" prop="Value">
      <el-input v-model="form.Value" />
    </el-form-item>
    <el-form-item label="所属类型" prop="TypeId">
      <el-select v-model="form.TypeId" placeholder="请选择">
        <el-option key="empty" label="--请选择--" value />
      </el-select>
    </el-form-item>
    <el-form-item label="是否默认项" prop="isDefault">
      <el-checkbox v-model="form.isDefault" />
    </el-form-item>
    <el-form-item label="备注" prop="Remark">
      <el-input v-model="form.Remark" type="textarea" :rows="2" placeholder="请输入备注" />
    </el-form-item>
    <el-form-item size="large" style="text-align:right;" :v-show="!form.disabled">
      <el-button type="primary" @click="btnSave">保存</el-button>
      <el-button
        type="succcess"
        :style="{'display':form.isDefault?'inline-block':'none'}"
        @click="btnPush"
      >一键下发</el-button>
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
        Code: null,
        Name: null,
        Value: null,
        TypeId: null,
        isDefault: false,
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
    btnPush: function() {},
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
