<template>
  <el-form ref="form" :model="form" label-width="80px" size="mini" :disabled="form.disabled">
    <el-form-item label prop="Id" style="display:none;">
      <el-input type="hidden" v-model="form.Id"></el-input>
    </el-form-item>
    <el-form-item label="姓名" prop="Name">
      <el-input v-model="form.Name"></el-input>
    </el-form-item>
    <el-form-item label="年龄" prop="Age">
      <el-input v-model="form.Age"></el-input>
    </el-form-item>
    <el-form-item label="身份证号" prop="IdCard">
      <el-input v-model="form.IdCard"></el-input>
    </el-form-item>
    <el-form-item label="昵称" prop="NickName">
      <el-input v-model="form.NickName"></el-input>
    </el-form-item>
    <el-form-item label="用户类型" prop="Type">
      <el-select v-model="form.Type" placeholder="请选择">
        <el-option key="empty" label="--请选择--" value></el-option>
        <el-option key="1" label="管理员" value="1"></el-option>
        <el-option key="2" label="普通用户" value="2"></el-option>
      </el-select>
    </el-form-item>
    <el-form-item size="large" style="text-align:right;" :v-show="!form.disabled">
      <el-button type="primary" @click="btnSave" >保存</el-button>
      <el-button @click="btnClose">关闭</el-button>
    </el-form-item>
  </el-form>
</template>

<script>
import request from "@/utils/action.js";

export default {
  name: "Edit",
  props: {
    options: Object
  },
  data: function() {
    return {
      form: {
        Id: this.options.id,
        disabled: false,
        Name: null,
        Age: null,
        IdCard: null,
        NickName: null,
        Type: null
      }
    };
  },
  methods: {
    btnSave: function() {
      debugger;
      let vm = this;
      request({
        url: "http://localhost:8016/User/Edit?key=" + vm.form.Id,
        type: "post",
        data: vm.form,
        success: function(data) {
          if (data.success) {
            vm.$refs.form.resetFields();
            vm.$emit("refresh", true);
          }
        }
      });
    },
    btnClose: function() {
      this.$emit("refresh", false);
    },
    initData: function() {
      //    信息初始化
      if (this.options.id) {
        let vm = this;
        request({
          url: "http://localhost:8016/User/GetUser?id=" + vm.options.id,
          type: "get",
          success: function(data) {
            if (data.success) {
              data.data.Type = data.data.Type.toString();
              vm.form = data.data;
              vm.form.disabled = vm.options.type === "detail";
            }
          },
          error: function(err) {
            console.log(err);
          }
        });
      }
    }
  }
};
</script>

<style scoped>
</style>