<script setup lang="tsx">
import { computed, ref } from 'vue';
import { useForm, useRequest } from 'alova/client';

import { useFormRules, useNaiveForm } from '@/hooks/common/form';
import { $t } from '@/locales';
import type { UpdateRoleParam } from '@/api/globals';

defineOptions({
  name: 'RoleEditForm'
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
    Apis.Role.post_api_role_submit({
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
    initialForm: {} as UpdateRoleParam
  }
);
/** 获取详情 */
const { send: getInfo } = useRequest(
  id =>
    Apis.Role.get_api_role_getinfo({
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
  <NModal v-model:show="visible" :title="title" preset="card" class="w-800px">
    <NScrollbar class="h-480px pr-20px">
      <NForm ref="formRef" :model="formData" :rules="rules" label-placement="left" :label-width="100">
        <NGrid responsive="screen" item-responsive>
          <NFormItemGi span="24 m:12" label="角色名称" path="name">
            <NInput v-model:value="formData.name" :placeholder="$t('common.placeholder')" />
          </NFormItemGi>
          <NFormItemGi span="24 m:12" label="角色编码" path="code">
            <NInput v-model:value="formData.code" :placeholder="$t('common.placeholder')" />
          </NFormItemGi>
          <NFormItemGi span="24 m:12" label="排序" path="sort">
            <NInputNumber v-model:value="formData.sort" :placeholder="$t('common.placeholder')" />
          </NFormItemGi>
          <NFormItemGi span="24 m:12" label="备注" path="remark">
            <NInput v-model:value="formData.remark" type="textarea" :placeholder="$t('common.placeholder')" />
          </NFormItemGi>
        </NGrid>
      </NForm>
    </NScrollbar>
    <template #footer>
      <NSpace justify="end" :size="16">
        <NButton @click="closeForm">{{ $t('common.cancel') }}</NButton>
        <NButton type="primary" @click="handleSubmit">{{ $t('common.confirm') }}</NButton>
      </NSpace>
    </template>
  </NModal>
</template>
