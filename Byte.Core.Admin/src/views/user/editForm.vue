<template>
  <el-dialog
    :title="title"
    v-model="visible"
    width="80%"
    destroy-on-close
    @close="close"
  >
    <el-form :model="form" ref="ruleFormRef" label-width="100px">
      <el-form-item label="头像" prop="image">
        <AvatarUpLoad v-model="form.avatar" />
      </el-form-item>
      <el-form-item label="账号" prop="account">
        <el-input
          v-model="form.account"
          autocomplete="off"
          placeholder="请输入账号"
        />
      </el-form-item>
      <!-- <el-form-item
        label="头像路径"
        prop="avatar"
        :rules="{
          required: true,
          message: '头像路径不能为空',
          trigger: ['blur', 'change'],
        }"
      /> -->
      <el-form-item label="角色" prop="roleId">
        <role-select style="width: 250px" v-model="form.roleIds" />
      </el-form-item>

      <el-form-item label="昵称" prop="name">
        <el-input
          v-model="form.name"
          autocomplete="off"
          placeholder="请输入昵称"
        />
      </el-form-item>
      <el-form-item label="电话号码" prop="phone">
        <el-input
          v-model="form.account"
          autocomplete="off"
          placeholder="请输入电话号码"
        />
      </el-form-item>
      <el-form-item label="密码" v-if="!form.id" prop="password">
        <el-input
          show-password
          v-model="form.password"
          autocomplete="off"
          placeholder="请输入密码"
        />
      </el-form-item>
      <el-form-item v-if="!form.id" label="确认密码" prop="newPassword">
        <el-input
          show-password
          v-model="form.newPassword"
          autocomplete="off"
          placeholder="请输入确认密码"
        />
      </el-form-item>
      <el-form-item label="状态" prop="state">
        <el-switch
          v-model="form.state"
          class="ml-2"
          inline-prompt
          style="--el-switch-on-color: #13ce66; --el-switch-off-color: #ff4949"
          active-text="启用"
          inactive-text="禁用"
        />
      </el-form-item>
    </el-form>
    <template #footer>
      <!-- <el-button
        :loading="loading"
        type="warning"
        @click="resetPassword"
        v-hasPerm="['user/setpassword']"
        v-if="form.id"
        style="float: left"
        ><i-ep-refresh />修改密码</el-button
      >
      <el-button @click="visible = false">取消</el-button> -->

      <el-button
        v-hasPerm="['user/update']"
        type="primary"
        @click="handleSubmit"
        :loading="loading"
      >
        保 存
      </el-button>
    </template>
  </el-dialog>
</template>
<script lang="ts" setup>
import { ref } from "vue";
import { ElMessage, FormInstance } from "element-plus";
import { DeptTypeEnum, getEnumValue } from "@/api/apiEnums";
import deptSelect from "@/views/dept/select.vue";
import { useRoute } from "vue-router";
import "@/api";
import { useForm } from "alova/client";
import { UpdateUserParam } from "@/api/globals";
import AvatarUpLoad from "@/components/Upload/AvatarUpLoad.vue";
import "@/api";
const route = useRoute();
const emit = defineEmits(["refresh"]);
/**
 * 获取详情
 */
const { send: getInfo } = useRequest(
  (id) =>
    Apis.User.get_api_user_getinfo({
      params: { id },
      transform: (res) => {
        updateForm(res.data);
      },
    }),
  { immediate: false }
);
/**
 * 提交详情
 */
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
        emit("refresh");
      },
    }),
  {
    immediate: false,
    resetAfterSubmiting: true,
    initialForm: {
      msgCode: 123456,
    } as UpdateUserParam & { newPassword?: string },
  }
);

const ruleFormRef = ref<FormInstance>();
const visible = ref(false);
const title = ref<string>(route.meta.title as string);
const loading = ref(false);
const close = () => {
  ruleFormRef.value?.resetFields();
  resetFrom();
};
//打开
const openForm = async (id?: string) => {
  visible.value = true;
  if (id) {
    await getInfo(id);
  } else {
    form.value.state = true;
  }
};
//保存
async function handleSubmit() {
  const valid: boolean | undefined = await ruleFormRef.value?.validate();
  if (!valid) {
    return;
  }
  await submit(form.value);
}
defineExpose({
  openForm,
});
</script>
