<template>
  <el-dialog
    :visible.sync="options.showDialog"
    :title="options.title"
    :width="options.width"
    :destroy-on-close="true"
    :show-close="true"
    :close-on-click-modal="false"
    @open="initData"
  >
    <el-form ref="form" :model="form" label-width="80px" size="mini" :rules="rules" :disabled="init.disabled">
      <el-row>
        <el-col :span="24">
          <el-form-item label="车辆">
            <el-input v-model="options.BusNo" style="width:200px;" :readonly="true" />
          </el-form-item>
        </el-col>
        <el-col :span="24">
          <el-form-item label="发送信息" prop="Content">
            <el-input v-model="form.Content" type="textarea" placeholder="请输入要发送的消息" style="width:200px;" rows="4" />
          </el-form-item>
        </el-col>
      </el-row>
      <el-form-item size="large" style="text-align:right;">
        <el-button type="primary" @click="btnQuery">发送</el-button>
        <el-button @click="btnClose">关闭</el-button>
      </el-form-item>
    </el-form>
  </el-dialog>
</template>

<script>
import request from '@/utils/action'
import communicate from '@/utils/communicate.js'
import datehelper from '@/utils/datehelper.js'
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
        Content: [
          { required: true, message: '发送内容不能为空', trigger: 'blur' }
        ]
      },
      init: {
        disabled: false
      },
      form: {
        SenderGUID: '',
        ReceiverGUID: '',
        ReceiverType: '0',
        SendTime: '',
        Content: '',
        Status: '-1'
      }
    }
  },
  methods: {
    //  保存数据
    btnQuery: function() {
      var vm = this
      // //  消息id
      // vm.form.SenderGUID = generate.uuid()
      // //  车辆Id
      // vm.form.ReceiverGUID = vm.options.ReceiverGUID
      // //  发送时间
      // vm.form.SendTime = datehelper.getCurrentTime()
      this.$refs.form.validate(valid => {
        if (valid) {
          var docId = vm.helpers.generate.uuid()
          var xml = `<Documents DataGuid="${docId}" DataType="NoticeInfo" TaskGuid="${vm.helpers.communicate.option.businessTaskGuid}"> `
          for (var index = 0; index < vm.options.ReceiverGUID.length; index++) {
            var item = vm.options.ReceiverGUID[index]
            var guid = vm.helpers.generate.uuid()
            var data = {
              Guid: guid,
              SenderGUID: guid,
              ReceiverGUID: item,
              ReceiverType: '0',
              SendTime: vm.helpers.date.getCurrentTime(),
              Content: vm.form.Content,
              Status: '-1'
            }
            xml += vm.helpers.convert.json2xml('SetData', vm.helpers.communicate.option.UserID, vm.helpers.communicate.option.businessTaskGuid, 'NoticeInfo', data, data.SenderGUID).XmlData
          }
          xml += '</Documents>'

          request({
            url:
          vm.helpers.communicate.option.businessUrl + '/SetData',
            type: 'post',
            data: {
              UserID: vm.helpers.communicate.option.UserID,
              TaskGuid: vm.helpers.communicate.option.businessTaskGuid,
              DataGuid: docId,
              DataType: 'NoticeInfo',
              XmlData: xml
            },
            success: (data) => {
              if (data.indexOf('</int>') >= 0) {
                var count = parseInt(data.substring(data.indexOf('">') + 2, data.indexOf('</int>')))
                if (count === 0) {
                  vm.msgSuccess('发送成功')
                  vm.$emit('callback', 'message', null)
                } else {
                  vm.msgError('发送失败')
                }
              }
            },
            error: (err) => {
              console.log('error : ', err)
            }
          })
        }
      })
      //  })
      // communicate.SetData('NoticeInfo', convert.null2empty(this.form), this.form.SenderGUID).then(data => {
      //   this.msgSuccess('发送成功')
      //   this.$emit('callback', 'message', this.form)
      // })
    },
    btnClose: function() {
      this.resetData()
      this.$emit('callback', 'message', this.form)
    },
    resetData: function() {
      this.form.SenderGUID = null
      this.form.ReceiverGUID = null
      this.form.SendTime = null
      this.form.Content = null
    },
    initData: function() {
      //  this.resetData()
      this.form.BusNo = this.options.BusNo.join(',')
    }
  }
}
</script>

<style scoped>
</style>

