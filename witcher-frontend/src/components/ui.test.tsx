import { render, screen } from '@testing-library/react'
import userEvent from '@testing-library/user-event'
import { afterEach, describe, expect, it, vi } from 'vitest'
import { ConfirmButton, Pagination } from './ui'

describe('Pagination', () => {
  it('renders nothing when the whole list fits on one page', () => {
    const { container } = render(<Pagination pageNumber={1} pageSize={10} totalCount={5} onPageChange={vi.fn()} />)
    expect(container).toBeEmptyDOMElement()
  })

  it('shows the current page out of the computed total', () => {
    render(<Pagination pageNumber={2} pageSize={10} totalCount={25} onPageChange={vi.fn()} />)
    expect(screen.getByText('Стр. 2 из 3')).toBeInTheDocument()
  })

  it('disables "Назад" on the first page and "Вперёд" on the last', () => {
    render(<Pagination pageNumber={1} pageSize={10} totalCount={25} onPageChange={vi.fn()} />)
    expect(screen.getByText('Назад')).toBeDisabled()
    expect(screen.getByText('Вперёд')).not.toBeDisabled()
  })

  it('calls onPageChange with the next page number', async () => {
    const onPageChange = vi.fn()
    const user = userEvent.setup()
    render(<Pagination pageNumber={2} pageSize={10} totalCount={25} onPageChange={onPageChange} />)

    await user.click(screen.getByText('Вперёд'))

    expect(onPageChange).toHaveBeenCalledWith(3)
  })
})

describe('ConfirmButton', () => {
  afterEach(() => {
    vi.restoreAllMocks()
  })

  it('calls onConfirm when the user accepts the browser confirm dialog', async () => {
    vi.spyOn(window, 'confirm').mockReturnValue(true)
    const onConfirm = vi.fn()
    const user = userEvent.setup()
    render(
      <ConfirmButton confirmMessage="Удалить?" onConfirm={onConfirm}>
        Удалить
      </ConfirmButton>,
    )

    await user.click(screen.getByText('Удалить'))

    expect(window.confirm).toHaveBeenCalledWith('Удалить?')
    expect(onConfirm).toHaveBeenCalledOnce()
  })

  it('does not call onConfirm when the user cancels the dialog', async () => {
    vi.spyOn(window, 'confirm').mockReturnValue(false)
    const onConfirm = vi.fn()
    const user = userEvent.setup()
    render(
      <ConfirmButton confirmMessage="Удалить?" onConfirm={onConfirm}>
        Удалить
      </ConfirmButton>,
    )

    await user.click(screen.getByText('Удалить'))

    expect(onConfirm).not.toHaveBeenCalled()
  })
})
