<script setup lang="ts">
import { computed, ref } from 'vue';
import { useForm, useRequest } from 'alova/client';
import { useRoute } from 'vue-router';
import { useFormRules, useNaiveForm } from '@/hooks/common/form';
import { $t } from '@/locales';
import { ColumnTypeEnum, ViewTypeEnum } from '@/api/apiEnums';
defineOptions({
  name: 'TableEditForm'
});
const route = useRoute();
const configId = ref(route.query.configId as string);
const tableof = ref(route.path.split('/').pop());
type FormDataType = Record<string, string>;

const formColumns = ref<Array<NaiveUI.TableColumnCheck>>([]);

const visible = ref<boolean>(false);

const { formRef, validate, restoreValidation } = useNaiveForm();
// 规则验证获取对象
const { defaultRequiredRule } = useFormRules();
// type RuleKey = keyof FormDataType;
const rules: Partial<Record<string, App.Global.FormRule>> = {};

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
    Apis.TableColumn.post_api_tablecolumn_submit_configid_tableof({
      pathParams: { configId: configId.value || '', tableof: tableof.value || '' },
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
    Apis.TableColumn.get_api_tablecolumn_getform_configid_tableof({
      pathParams: { configId: configId.value || '', tableof: tableof.value || '' },
      params: { id },
      transform: res => {
        updateForm(res.data || {});
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
      <TableHeaderSetting
        v-model:columns="formColumns"
        :config-id="configId"
        :tableof="tableof"
        :view-type="ViewTypeEnum.编辑页"
      ></TableHeaderSetting>

      <NForm ref="formRef" :model="formData" :rules="rules" label-placement="left" :label-width="80">
        <NFormItem
          v-for="(item, index) in formColumns"
          :key="index"
          span="24 s:12 m:6"
          :label="$t(item.title)"
          :path="item.key"
          class="pr-24px"
        >
          <DicSelect
            v-if="item.columnType === ColumnTypeEnum.字典"
            v-model:value="formData[item.key || '']"
            :group-by="item.columnTypeDetail"
          ></DicSelect>
          <EnumSelect
            v-else-if="item.columnType === ColumnTypeEnum.枚举"
            v-model:value="formData[item.key || '']"
            :group-by="item.columnTypeDetail"
          ></EnumSelect>

          <NInput
            v-else
            v-model:value="formData[item.key || '']"
            :placeholder="$t('common.placeholder') + $t(item.title)"
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
