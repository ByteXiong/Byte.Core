<script setup lang="ts">
import { computed, ref, shallowRef } from 'vue';
import { useForm, useRequest } from 'alova/client';
import { $t } from '@/locales';
import type { SetByRoleIdDTO } from '@/api/globals';
defineOptions({
  name: 'MenuTree'
});
const visible = ref<boolean>(false);

interface Emits {
  (e: 'refresh', row: any): any;
}
const emit = defineEmits<Emits>();

/** 获取数据Tree */
const { data, loading } = useRequest(
  () =>
    Apis.Menu.get_api_menu_select({
      transform: res => {
        return res.data;
      }
    }),
  {
    immediate: true
  }
);

/** 提交详情 */
const {
  send: handleSubmit,
  form: formData,
  reset: resetFrom,
  updateForm
} = useForm(
  form =>
    Apis.Menu.post_api_menu_setbyroleid({
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
    initialForm: {} as SetByRoleIdDTO
  }
);
/** 获取数据getbyroleid */
const { send: getData, loading: dataLoading } = useRequest(
  roleId =>
    Apis.Menu.get_api_menu_getbyroleid({
      params: { roleId },
      transform: res => {
        updateForm({ roleId, menuIds: res.data });
      }
    }),
  {
    immediate: true
  }
);
const title = computed(() => {
  return '角色权限';
});
// 打开
const openForm = async (id?: string) => {
  visible.value = true;
  if (id) {
    await getData(id);
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
  <NModal v-model:show="visible" :title="title" preset="card" class="w-480px">
    <NTree
      v-model:checked-keys="formData.menuIds"
      :loading="loading && dataLoading"
      :data="data"
      label-field="title"
      key-field="id"
      checkable
      expand-on-click
      virtual-scroll
      block-line
      class="h-280px"
    />
    <template #footer>
      <NSpace justify="end">
        <NButton size="small" class="mt-16px" @click="closeForm">
          {{ $t('common.cancel') }}
        </NButton>
        <NButton type="primary" size="small" class="mt-16px" @click="handleSubmit">
          {{ $t('common.confirm') }}
        </NButton>
      </NSpace>
    </template>
  </NModal>
</template>

<style scoped></style>
