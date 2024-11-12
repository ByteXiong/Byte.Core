<script setup lang="ts">
import { computed, ref } from 'vue';
import { useForm, useRequest } from 'alova/client';
import { useFormRules, useNaiveForm } from '@/hooks/common/form';
import { $t } from '@/locales';
import type { UpdateUserParam } from '@/api/globals';
import RoleSelect from './role-select.vue';
defineOptions({
  name: 'UserEditForm'
});
type Model = Pick<
  Api.SystemManage.User,
  'userName' | 'userGender' | 'nickName' | 'userPhone' | 'userEmail' | 'userRoles' | 'status'
>;

const visible = ref<boolean>(false);
const { formRef, validate, restoreValidation } = useNaiveForm();
type RuleKey = Extract<keyof Model, 'userName' | 'status'>;
const { defaultRequiredRule } = useFormRules();
const rules: Record<RuleKey, App.Global.FormRule> = {
  userName: defaultRequiredRule,
  status: defaultRequiredRule
};

interface Emits {
  (e: 'refresh', row: any): any;
}
const emit = defineEmits<Emits>();

/** 提交详情 */
const {
  send: handleSubmit,
  form: formData,
  reset: resetFrom,
  updateForm
} = useForm(
  form =>
    Apis.User.post_api_user_submit({
      data: form,
      transform: () => {
        visible.value = false;
        window.$message?.success($t('common.updateSuccess'));
        emit('refresh', form);
      }
    }),
  {
    immediate: false,
    resetAfterSubmiting: true,
    initialForm: {} as UpdateUserParam
  }
);
/** 获取详情 */
const { send: getInfo } = useRequest(
  id =>
    Apis.User.get_api_user_getinfo({
      params: { id },
      transform: res => {
        updateForm(res.data);
      }
    }),
  { immediate: false }
);
const title = computed(() => {
  return formData.value.id ? $t('common.add') : $t('common.edit');
});
// 打开
const openForm = async (id?: string) => {
  visible.value = true;
  if (id) {
    await getInfo(id);
  } else {
    formData.value.status = true;
  }
};
const closeForm = () => {
  restoreValidation();
  resetFrom();
};

defineExpose({
  openForm
});
</script>

<template>
  <NDrawer v-model:show="visible" display-directive="show" :width="360">
    <NDrawerContent :title="title" :native-scrollbar="false" closable>
      <NForm ref="formRef" :model="formData" :rules="rules">
        <NFormItem label="账号名" path="userName">
          <NInput v-model:value="formData.userName" :placeholder="$t('common.placeholder')" />
        </NFormItem>

        <NFormItem :label="$t('page.manage.user.nickName')" path="nickName">
          <NInput v-model:value="formData.nickName" :placeholder="$t('common.placeholder')" />
        </NFormItem>
        <NFormItem :label="$t('page.manage.user.userPhone')" path="phone">
          <NInput v-model:value="formData.phone" :placeholder="$t('common.placeholder')" />
        </NFormItem>
        <NFormItem :label="$t('page.manage.user.userEmail')" path="email">
          <NInput v-model:value="formData.email" :placeholder="$t('common.placeholder')" />
        </NFormItem>
        <NFormItem :label="$t('page.manage.user.userStatus')" path="status">
          <NSwitch v-model:value="formData.status">
            <template #checked>启用</template>
            <template #unchecked>禁用</template>
          </NSwitch>
        </NFormItem>

        <NFormItem label="用户角色" path="roleIds">
          <RoleSelect v-model:value="formData.roleIds" multiple></RoleSelect>
        </NFormItem>
      </NForm>
      <template #footer>
        <NSpace :size="16">
          <NButton @click="closeForm">{{ $t('common.cancel') }}</NButton>
          <NButton type="primary" @click="handleSubmit">{{ $t('common.confirm') }}</NButton>
        </NSpace>
      </template>
    </NDrawerContent>
  </NDrawer>
</template>
