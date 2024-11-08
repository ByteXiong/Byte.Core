<script setup lang="ts">
import { computed, ref, watch } from 'vue';
import { $t } from '@/locales';
import { useFormRules, useNaiveForm } from '@/hooks/common/form';
import type { TableColumn } from '@/api/globals';
import { ColumnTypeEnum } from '@/api/apiEnums';
defineOptions({
  name: 'TableHeaderSearch'
});

interface Emits {
  (e: 'reset'): void;
  (e: 'search'): void;
}

const emit = defineEmits<Emits>();

const { formRef, validate, restoreValidation } = useNaiveForm();

const searchParams = defineModel<NaiveUI.SearchParams>('searchParams', {
  required: true
});

const formDada = ref<Record<string, any>>({});
// const params = ref<Record<string, NaiveUI.SearchField>>({} as { [key: string]: NaiveUI.SearchField });

type RuleKey = Extract<keyof Api.SystemManage.UserSearchParams, 'userEmail' | 'userPhone'>;

const rules = computed<Record<RuleKey, App.Global.FormRule>>(() => {
  const { patternRules } = useFormRules(); // inside computed to make locale reactive

  return {
    userEmail: patternRules.email,
    userPhone: patternRules.phone
  };
});

interface Props {
  searchData: TableColumn[];
}

const props = defineProps<Props>();

async function reset() {
  await restoreValidation();
  emit('reset');
}
watch(
  () => formDada.value,
  newVal => {
    Object.keys(newVal).forEach(key => {
      searchParams.value[key] = {
        key,
        searchType: props.searchData.find(item => item.key === key)?.searchType,
        value: newVal[key]
      };
    });
    // searchParams.value = newVal;
  },
  { deep: true }
);

async function search() {
  await validate();

  emit('search');
}
</script>

<template>
  <NCard :bordered="false" size="small" class="card-wrapper">
    {{ searchParams }}
    ===================================
    {{ formDada }}
    <!--
 <NCollapse>
      <NCollapseItem :title="$t('common.search')" name="user-search">
-->
    <NForm ref="formRef" :model="formDada" :rules="rules" label-placement="left" :label-width="80">
      <NGrid responsive="screen" item-responsive>
        <NFormItemGi
          v-for="(item, index) in searchData"
          :key="index"
          span="24 s:12 m:6"
          :label="$t(item.title)"
          :path="item.key"
          class="pr-24px"
        >
          <DicSelect
            v-if="item.columnType === ColumnTypeEnum.字典"
            v-model:value="formDada[item.key || '']"
            :group-by="item.columnTypeDetail"
          ></DicSelect>
          <NInput
            v-else
            v-model:value="formDada[item.key || '']"
            :placeholder="$t('common.placeholder') + $t(item.title)"
          />
        </NFormItemGi>
        <NFormItemGi span="24 m:12" class="pr-24px">
          <NSpace class="w-full" justify="end">
            <NButton @click="reset">
              <template #icon>
                <icon-ic-round-refresh class="text-icon" />
              </template>
              {{ $t('common.reset') }}
            </NButton>
            <NButton type="primary" ghost @click="search">
              <template #icon>
                <icon-ic-round-search class="text-icon" />
              </template>
              {{ $t('common.search') }}
            </NButton>
          </NSpace>
        </NFormItemGi>
      </NGrid>
    </NForm>
    <!--
 </NCollapseItem>
    </NCollapse>
-->
  </NCard>
</template>

<style scoped></style>
