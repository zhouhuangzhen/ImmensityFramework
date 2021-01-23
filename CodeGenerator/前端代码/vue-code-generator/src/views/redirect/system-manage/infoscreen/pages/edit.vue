<template>
  <el-form ref="form" :model="form" label-width="80px" size="mini" :disabled="form.disabled">
    <el-form-item label prop="Id" style="display:none;">
      <el-input type="hidden" v-model="form.Id"></el-input>
    </el-form-item>
    <el-form-item label="首末站" prop="Station">
      <el-select v-model="form.Station" :filterable="true" placeholder="请选择">
        <el-option
          v-for="item in station"
          :key="item.value"
          :label="item.label"
          :value="item.value"
        ></el-option>
      </el-select>
    </el-form-item>
    <el-form-item label="线路" prop="Line">
      <el-select
        ref="lines"
        v-model="form.Line"
        :filterable="true"
        placeholder="请选择"
        :multiple="true"
      >
        <el-option v-for="item in line" :key="item.value" :label="item.label" :value="item.value"></el-option>
      </el-select>
    </el-form-item>
    <el-form-item label="顺序" prop="Order">
      <el-input-number v-model="form.Order" :min="1" :max="10" label="顺序"></el-input-number>
    </el-form-item>
    <el-form-item style="text-align:right;">
      <el-button @click="btnAddLine">添加线路</el-button>
    </el-form-item>
    <el-form-item label="信息列表" label-position="top">
      <el-table
        :border="true"
        :data="data"
        style="width:100%;"
        ref="table"
        key="table"
        :fit="true"
        :stripe="true"
        empty-text="暂无数据"
      >
        <el-table-column type="index" label="序号" align="center" width="80"></el-table-column>
        <el-table-column type="text" v-if="false" property="LineId"></el-table-column>
        <el-table-column
          key="LineName"
          type="text"
          label="线路信息"
          property="LineName"
          :sortable="true"
          :resizable="true"
          :show-overflow-tooltip="true"
          align="left"
        ></el-table-column>
      </el-table>
    </el-form-item>
    <el-form-item size="large" style="text-align:right;" :v-show="!form.disabled">
      <el-button type="primary" @click="btnSave">保存</el-button>
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
        Id: this.options.Id,
        disabled: false,
        Station: null,
        Line: null,
        Order: null
      },
      station: [
        {
          value: 1,
          label: "东南环公交首末站"
        },
        {
          value: 2,
          label: "东方大道北"
        },
        {
          value: 3,
          label: "向阳桥北"
        },
        {
          value: 4,
          label: "花苑街南"
        },
        {
          value: 5,
          label: "东山路"
        }
      ],
      line: [
        {
          value: 1,
          label: "103上行"
        },
        {
          value: 2,
          label: "205上行"
        },
        {
          value: 3,
          label: "312下行"
        },
        {
          value: 4,
          label: "115上行"
        },
        {
          value: 5,
          label: "232下行"
        }
      ],
      data: []
    };
  },
  methods: {
    btnSave: function() {
      console.log(this.$refs.table);
      return ;
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
      if (this.options.Id) {
        let vm = this;
        request({
          url: "http://localhost:8016/User/GetUser?id=" + vm.options.Id,
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
    },
    btnAddLine: function() {
      console.log(this.$refs.lines);
      this.$refs.lines.selected.forEach(item => {
        this.data.push({
          LineId: item.value,
          LineName: item.label
        });
      });
    }
  }
};
</script>

<style scoped>
</style>