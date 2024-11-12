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
type FormDataType = UpdateUserParam;

const visible = ref<boolean>(false);

const { formRef, validate, restoreValidation } = useNaiveForm();
// 规则验证获取对象
const { defaultRequiredRule } = useFormRules();
type RuleKey = keyof FormDataType;
const rules: Partial<Record<RuleKey, App.Global.FormRule>> = {
  userName: defaultRequiredRule
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
    initialForm: {} as FormDataType,
    async middleware(_, next) {
      validate().then(async () => {
        await next();
      });
    }
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
  { force: true, immediate: false }
);
const title = computed(() => {
  return formData.value.id ? $t('common.edit') : $t('common.add');
});
// 打开
const openForm = async (id?: string) => {
  visible.value = true;
  if (id) {
    await getInfo(id);
  }
};
const closeForm = () => {
  // 关闭页面 清空formData
  visible.value = false;
  restoreValidation();
  resetFrom();
};
defineExpose({
  openForm
});
</script>

<template>
  <NModal v-model:show="visible" :title="title" preset="card" class="w-800px" @after-leave="closeForm">
    <NScrollbar class="h-480px pr-20px">
      <NForm ref="formRef" :model="formData" :rules="rules" label-placement="left" :label-width="100">
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
          <NRadioGroup v-model:value="formData.status">
            <NRadio :value="true" label="启用" />
            <NRadio :value="false" label="禁用" />
          </NRadioGroup>
        </NFormItem>

        <NFormItem label="用户角色" path="roleIds">
          <RoleSelect v-model:value="formData.roleIds" multiple></RoleSelect>
        </NFormItem>
      </NForm>
    </NScrollbar>
    <template #footer>
      <NSpace :size="16">
        <NButton @click="closeForm">{{ $t('common.cancel') }}</NButton>
        <NButton type="primary" @click="handleSubmit">{{ $t('common.confirm') }}</NButton>
      </NSpace>
    </template>
  </NModal>
</template>
