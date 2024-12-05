<script setup lang="tsx">
import { ref } from 'vue';
import { NButton, NModal } from 'naive-ui';
import { $t } from '@/locales';
import { useForm } from '~/packages/alova/src/client';
import customRender from '@/utils/customRender';
const visible = ref(false);
const title = ref('数据表格高阶渲染');
const { columnId } = defineProps<{ columnId: number }>();
/** 提交详情 */
const {
  send: handleSubmit,
  form: formData,
  reset: resetFrom,
  updateForm
} = useForm(
  form =>
    Apis.TableView.put_api_tableview_setprops({
      data: {
        columnId,
        props: JSON.stringify(form)
      },
      transform: () => {
        visible.value = false;
        window.$message?.success($t('common.updateSuccess'));
        // emit('refresh', form);
      }
    }),
  {
    immediate: false,
    resetAfterSubmiting: true,
    initialForm: {
      key: '',
      title: '',
      render: ''
    }
    // async middleware(_, next) {
    //   validate().then(async () => {
    //     await next();
    //   });
    // }
  }
);

const props = defineModel<string>('props', {
  required: true
});
updateForm(customRender(props.value));

// const handleSubmit = () => {
//   visible.value = false;
// };

const closeModal = () => {
  resetFrom();
  visible.value = false;
};
const handClick = () => {
  visible.value = true;
};

// const formData = ref<DataTableColumn>({
//   key: '',
//   title: '',
//   align: 'center'
// });
</script>

<template>
  <NButton type="primary" :ghost="!props || props.length == 0" size="small" @click="handClick">高阶配置</NButton>

  <NModal v-model:show="visible" :title="title" preset="card" class="w-1800px">
    <NGrid :x-gap="12" :y-gap="12" :cols="8">
      <NGi span="2">
        {{ columnId }}
        {{ formData }}
        <!--
 <NDataTable
          :columns="columns"
          :data="tableData"
          size="small"
          remote
          :row-key="row => row.id"
          :loading="loading"
        />
-->
      </NGi>
      <NGi :span="4"></NGi>
      <NGi :span="2">
        <NScrollbar class="h-750px pr-20px">
          <NForm ref="formRef" :model="formData" label-placement="left" :label-width="80">
            <NFormItem span="24 s:12 m:6" label="键" path="key" class="pr-24px">
              <NInput v-model:value="formData.key" :placeholder="$t('common.placeholder')" />
            </NFormItem>
            <NFormItem span="24 s:12 m:6" label="名称" path="title" class="pr-24px">
              <NInput v-model:value="formData.title" :placeholder="$t('common.placeholder')" />
            </NFormItem>

            <NFormItem span="24 s:12 m:6" label="排序" path="sort" class="pr-24px">
              <NInput v-model:value="formData.sort" :placeholder="$t('common.placeholder')" />
            </NFormItem>

            <NFormItem span="24 s:12 m:6" label="渲染" path="render" class="pr-24px">
              <NInput v-model:value="formData.render" :placeholder="$t('common.placeholder')" />
            </NFormItem>
          </NForm>
        </NScrollbar>
      </NGi>
    </NGrid>
    <template #footer>
      <NSpace justify="space-between" :size="16">
        <NSpace justify="end" :size="16">
          <NButton type="primary" @click="closeModal">{{ $t('common.cancel') }}</NButton>
          <NButton type="primary" @click="handleSubmit">{{ $t('common.confirm') }}</NButton>
        </NSpace>
      </NSpace>
    </template>
  </NModal>
</template>

<style lang="less" scoped></style>
