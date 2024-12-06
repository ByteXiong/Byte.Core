<script setup lang="ts">
import { computed, ref } from 'vue';
import { useForm } from 'alova/client';
import { useFormRules, useNaiveForm } from '@/hooks/common/form';
import { $t } from '@/locales';
import type { SetPasswordParam } from '@/api/globals';

defineOptions({
  name: 'SetPasswordForm'
});
type FormDataType = typeof formData.value;

const visible = ref<boolean>(false);

const { formRef, validate, restoreValidation } = useNaiveForm();
// 规则验证获取对象
const { defaultRequiredRule, patternRules } = useFormRules();
type RuleKey = keyof FormDataType;
const rules: Partial<Record<RuleKey, App.Global.FormRule | App.Global.FormRule[]>> = {
  oldPassword: [defaultRequiredRule, patternRules.pwd],
  newPassword: [defaultRequiredRule, patternRules.pwd],
  newPassword2: [defaultRequiredRule, patternRules.pwd]
};
const id = ref<number>(0);
interface Emits {
  (e: 'refresh', row: any): any;
}
const emit = defineEmits<Emits>();

/** 提交详情 */
const {
  send: handleSubmit,
  form: formData,
  reset: resetFrom
} = useForm(
  form =>
    (id.value ? Apis.User.put_api_user_setpassword : Apis.Login.put_api_login_setpassword)({
      data: { id: id.value, ...form },
      transform: () => {
        visible.value = false;
        window.$message?.success($t('common.updateSuccess'));
        emit('refresh', form);
      }
    }),
  {
    immediate: false,
    resetAfterSubmiting: true,
    initialForm: {} as SetPasswordParam & {
      newPassword2: '';
    },
    async middleware(_, next) {
      validate().then(async () => {
        await next();
      });
    }
  }
);

const title = computed(() => {
  return '修改密码';
});
// 打开
const openForm = async (userid?: number) => {
  id.value = userid || 0;
  visible.value = true;
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
        <NFormItem :label="$t('旧密码')" path="oldPassword">
          <NInput
            v-model:value="formData.oldPassword"
            type="password"
            show-password-on="click"
            :placeholder="$t('common.placeholder')"
          />
        </NFormItem>
        <NFormItem :label="$t('新密码')" path="newPassword">
          <NInput
            v-model:value="formData.newPassword"
            type="password"
            show-password-on="click"
            :placeholder="$t('common.placeholder')"
          />
        </NFormItem>

        <NFormItem :label="$t('确认密码')" path="newPassword2">
          <NInput
            v-model:value="formData.newPassword2"
            type="password"
            show-password-on="click"
            :placeholder="$t('common.placeholder')"
          />
        </NFormItem>
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
