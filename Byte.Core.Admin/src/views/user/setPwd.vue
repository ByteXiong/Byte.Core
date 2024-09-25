<template>
  <el-dialog
    :title="title"
    v-model="visible"
    width="80%"
    destroy-on-close
    @close="close"
  >
    <el-form
      ref="ruleFormRef"
      :model="formData"
      :rules="rules"
      label-width="80px"
      @submit.prevent="submit"
    >
      <el-form-item label="旧密码" prop="oldPwd">
        <el-input v-model="formData.oldPwd" type="password" />
      </el-form-item>
      <el-form-item label="新密码" prop="newPwd">
        <el-input v-model="formData.newPwd" type="password" />
      </el-form-item>
      <el-form-item>
        <el-button type="primary" @click="submit">确定</el-button>
        <el-button @click="ruleFormRef?.resetFields()">重置</el-button>
      </el-form-item>
    </el-form>
  </el-dialog>
</template>

<script setup lang="ts">
import { ref, reactive } from "vue";
import { ElMessage, FormInstance, FormRules } from "element-plus";
const title = ref("个人信息");
const visible = ref(false);
const close = () => {
  visible.value = false;
};

const ruleFormRef = ref<FormInstance>();
const rules = reactive<FormRules>({
  oldPwd: [
    { required: true, message: "请输入旧密码", trigger: "blur" },
    { min: 6, max: 20, message: "长度在 6 到 20 个字符", trigger: "blur" },
  ],
  newPwd: [
    { required: true, message: "请输入新密码", trigger: "blur" },
    { min: 6, max: 20, message: "长度在 6 到 20 个字符", trigger: "blur" },
  ],
});
const formData = reactive({
  oldPwd: "",
  newPwd: "",
});
// const submit = () => {
//   ruleFormRef.value?.validate((valid) => {
//     if (valid) {
//       ElMessage.success("修改成功");
//     } else {
//       return false;
//     }
//   });
// };

const {
  send: submit,
  form,
  reset: resetFrom,
  updateForm,
} = useForm(
  (form) =>
    Apis.User.post_api_user_submit({
      data: form,
      transform: () => {
        visible.value = false;
        ElMessage.success("保存成功！");
      },
    }),
  {
    immediate: false,
    resetAfterSubmiting: true,
    initialForm: {},
  }
);

defineExpose({
  visible,
});
</script>
