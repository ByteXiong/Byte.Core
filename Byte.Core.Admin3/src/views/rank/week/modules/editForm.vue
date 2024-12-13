<script setup lang="ts">
import { computed, ref } from 'vue';
import { useForm } from 'alova/client';
import dayjs from 'dayjs';
import { useFormRules, useNaiveForm } from '@/hooks/common/form';
import { $t } from '@/locales';
import type { SetWinParam } from '@/api/globals';
defineOptions({
  name: 'RankWeekEditForm'
});
type FormDataType = SetWinParam;

const visible = ref<boolean>(false);

const { formRef, validate, restoreValidation } = useNaiveForm();
// 规则验证获取对象
const { defaultRequiredRule } = useFormRules();
type RuleKey = keyof FormDataType;
const rules: Partial<Record<RuleKey, App.Global.FormRule | App.Global.FormRule[]>> = {
  newWin: defaultRequiredRule,
  newGems: defaultRequiredRule
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
    Apis.Rank.put_api_rank_setwin({
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
    initialForm: {} as FormDataType & {
      nickName: '';
      win: 0;
      gems: 0;
      date: '';
    },
    async middleware(_, next) {
      validate().then(async () => {
        await next();
      });
    }
  }
);
const title = computed(() => {
  return '编辑-周排行榜';
});
// 打开
const openForm = async (row: any, timestamp: string) => {
  visible.value = true;
  updateForm({ ...row, type: 2, date: dayjs(timestamp).format('YYYY-MM-DD HH:mm:ss') });
};
const closeForm = () => {
  // 关闭页面 清空formData
  /** ✨ Codeium Command ⭐ ************ */
  /** Closes the form dialog, resets the form data, and clears the validation state. */
  /** 38810830-7b81-406c-81b5-f9f7d5c0ce8e ****** */
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
        <NFormItem label="昵称" path="nickName">
          {{ formData.nickName }} 日期:{{ dayjs(formData.timestamp).format('YYYY-MM-DD') }}
        </NFormItem>

        <NFormItem label="原宝石数" path="gems">
          {{ formData.gems }}
        </NFormItem>
        <NFormItem label="原连胜场次" path="gems">
          {{ formData.win }}
        </NFormItem>

        <NFormItem label="修改宝石数" path="newGems">
          <NInputNumber v-model:value="formData.newGems" :placeholder="$t('common.placeholder')" />
        </NFormItem>

        <NFormItem label="修改连胜场次" path="newWin">
          <NInputNumber v-model:value="formData.newWin" :placeholder="$t('common.placeholder')" />
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
