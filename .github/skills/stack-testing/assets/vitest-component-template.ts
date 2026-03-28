import { describe, it, expect, vi, beforeEach } from 'vitest'
import { mount } from '@vue/test-utils'
import { createPinia, setActivePinia } from 'pinia'
import ItemForm from '@/components/ItemForm.vue'
import { useItemStore } from '@/stores/items'

describe('ItemForm.vue', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
  })

  it('renders form with required fields', () => {
    const wrapper = mount(ItemForm)
    
    expect(wrapper.find('input[name="name"]').exists()).toBe(true)
    expect(wrapper.find('input[name="price"]').exists()).toBe(true)
    expect(wrapper.find('button[type="submit"]').exists()).toBe(true)
  })

  it('submits form with valid data', async () => {
    const wrapper = mount(ItemForm)
    const store = useItemStore()
    
    vi.spyOn(store, 'createItem').mockResolvedValue({ id: '123' })

    await wrapper.find('input[name="name"]').setValue('Test Item')
    await wrapper.find('input[name="price"]').setValue('99.99')
    await wrapper.find('form').trigger('submit')

    expect(store.createItem).toHaveBeenCalledWith(
      expect.objectContaining({
        name: 'Test Item',
        price: 99.99
      })
    )
  })

  it('shows validation errors for empty fields', async () => {
    const wrapper = mount(ItemForm)
    
    await wrapper.find('button[type="submit"]').trigger('click')

    expect(wrapper.find('.error-message').exists()).toBe(true)
  })

  it('emits success event after creation', async () => {
    const wrapper = mount(ItemForm)
    const store = useItemStore()
    
    vi.spyOn(store, 'createItem').mockResolvedValue({ id: '123' })

    await wrapper.find('input[name="name"]').setValue('Test Item')
    await wrapper.find('input[name="price"]').setValue('99.99')
    await wrapper.find('form').trigger('submit')
    await wrapper.vm.$nextTick()

    expect(wrapper.emitted('item-created')).toBeTruthy()
  })
})
