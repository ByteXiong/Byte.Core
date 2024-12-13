<script setup lang="ts">
import { computed, onMounted, ref } from 'vue';
import { useForm, useRequest } from 'alova/client';
import { useRoute } from 'vue-router';
import { useNaiveForm } from '@/hooks/common/form';
import { $t } from '@/locales';
import { ColumnTypeEnum, ViewTypeEnum } from '@/api/apiEnums';
import { alovaInstance } from '@/api';
defineOptions({
  name: 'TableEditForm'
});
const route = useRoute();

interface Props {
  configId: string;
  tableof: string;
  infoUrl: string;
  submitUrl: string;
}
const { configId, tableof, infoUrl, submitUrl } = defineProps<Props>();

// const configId = ref(route.query.configId as string);
// const tableof = ref(route.path.split('/').pop());
type FormDataType = Record<string, string>;

const formColumns = ref<Array<NaiveUI.TableColumnCheck>>([]);

const visible = ref<boolean>(false);

const { formRef, validate, restoreValidation } = useNaiveForm();
// 规则验证获取对象
// const { defaultRequiredRule } = useFormRules();
// type RuleKey = keyof FormDataType;
// const rules: Partial<Record<string, App.Global.FormRule>> = {};

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
    alovaInstance.Post(submitUrl, {
      // Apis.TableColumn.post_api_tablecolumn_submit_configid_tableof({
      //   pathParams: { configId: configId || '', tableof: tableof || '' },
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
    // Apis.TableColumn.get_api_tablecolumn_getform_configid_tableof({
    //   pathParams: { configId: configId.value || '', tableof: tableof.value || '' },
    alovaInstance.Get(infoUrl, {
      params: { id },
      transform: (res: any) => {
        updateForm(res.data || {});
      }
    }),
  { force: true, immediate: false }
);
const title = computed(() => {
  return (formData.value.id ? $t('common.edit') : $t('common.add')) + $t(route.meta.i18nKey || route.meta.title);
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
//= ======================富文本编辑器==========================
// const editor = ref<WangEditor>();
// const domRef = ref<HTMLElement>();

// function renderWangEditor() {
//   editor.value = new WangEditor(domRef.value);
//   setEditorConfig();
//   editor.value.create();
// }

// function setEditorConfig() {
//   if (editor.value?.config?.zIndex) {
//     editor.value.config.zIndex = 10;
//   }
// }
const dataTableConfig = ref<NaiveUI.dataTableConfig>();
onMounted(() => {
  // renderWangEditor();
});
</script>

<template>
  <NModal v-model:show="visible" :title="title" preset="card" class="w-800px" @after-leave="closeForm">
    <NScrollbar class="h-480px pr-20px">
      <TableHeaderSetting
        v-model:columns="formColumns"
        v-model:data-table-config="dataTableConfig"
        :config-id="configId"
        :tableof="tableof"
        :view-type="ViewTypeEnum.编辑"
      ></TableHeaderSetting>
      {{ infoUrl }}
      <NForm ref="formRef" :model="formData" label-placement="left" :label-width="80">
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
            v-else-if="item.columnType === ColumnTypeEnum.TexTarea文本"
            v-model:value="formData[item.key || '']"
            type="textarea"
            :autosize="{ minRows: 1, maxRows: 7 }"
          />

          <WangEditor
            v-else-if="item.columnType === ColumnTypeEnum.富文本"
            v-model:value="formData[item.key || '']"
          ></WangEditor>

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
