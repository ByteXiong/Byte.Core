<script setup lang="tsx">
import { computed, ref } from 'vue';
import { useForm, useRequest } from 'alova/client';

import { useNaiveForm } from '@/hooks/common/form';
import { $t } from '@/locales';
import type { UpdateRoleParam } from '@/api/globals';

defineOptions({
  name: 'RoleEditForm'
});

const visible = ref<boolean>(false);

const { formRef, validate, restoreValidation } = useNaiveForm();
// 规则验证获取对象
// const { defaultRequiredRule } = useFormRules();
type RuleKey = keyof typeof formData.value;
const rules: Partial<Record<RuleKey, App.Global.FormRule>> = {};
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
    initialForm: {
      status: true
    } as UpdateRoleParam,
    async middleware(_, next) {
      validate();
      await next();
    }
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
  {
    force: true,
    immediate: false
  }
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
        <NGrid responsive="screen" item-responsive>
          <NFormItemGi span="24 m:12" label="角色名称" path="name">
            <NInput v-model:value="formData.name" :placeholder="$t('common.placeholder')" />
          </NFormItemGi>
          <NFormItemGi span="24 m:12" label="角色编码" path="code">
            <NInput v-model:value="formData.code" :placeholder="$t('common.placeholder')" />
          </NFormItemGi>
          <NFormItemGi span="24 m:12" label="状态" path="status">
            <NRadioGroup v-model:value="formData.status">
              <NRadio :value="true" label="启用" />
              <NRadio :value="false" label="禁用" />
            </NRadioGroup>
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
