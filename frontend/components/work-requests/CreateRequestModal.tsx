'use client';

import { useState } from 'react';
import { workRequestApi } from '@/services/api';
import { Priority, WorkRequestStatus } from '@/types/workRequest';

interface CreateRequestModalProps {
  isOpen: boolean;
  onClose: () => void;
  onCreated: () => void;
}

const initialForm = {
  title: '',
  clientName: '',
  description: '',
  priority: 'Medium' as Priority,
  status: 'New' as WorkRequestStatus,
  dueDate: '',
};

export default function CreateRequestModal({
  isOpen,
  onClose,
  onCreated,
}: CreateRequestModalProps) {
  const [form, setForm] = useState(initialForm);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [error, setError] = useState<string | null>(null);

  if (!isOpen) return null;

  const todayStr = new Date().toISOString().split('T')[0];

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>
  ) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    // Client-side validation
    if (!form.title.trim()) return setError('Title is required.');
    if (!form.clientName.trim()) return setError('Client Name is required.');
    if (!form.description.trim()) return setError('Description is required.');
    if (!form.dueDate) return setError('Due Date is required.');

    setIsSubmitting(true);
    try {
      await workRequestApi.create({
        ...form,
        dueDate: new Date(form.dueDate).toISOString(),
      });
      setForm(initialForm);
      onCreated();
      onClose();
    } catch (err) {
      setError(err instanceof Error ? err.message : 'Something went wrong.');
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleClose = () => {
    setForm(initialForm);
    setError(null);
    onClose();
  };

  const inputClass =
    'w-full rounded-lg border border-gray-700 bg-gray-800 px-3 py-2.5 text-sm text-gray-100 placeholder-gray-500 outline-none transition-colors focus:border-indigo-500 focus:ring-1 focus:ring-indigo-500';
  const labelClass = 'block text-sm font-medium text-gray-300 mb-1.5';

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center">
      {/* Overlay */}
      <div
        className="absolute inset-0 bg-black/60 backdrop-blur-sm"
        onClick={handleClose}
      />

      {/* Modal */}
      <div className="relative w-full max-w-lg rounded-2xl border border-gray-800 bg-gray-900 p-6 shadow-2xl">
        {/* Close button */}
        <button
          onClick={handleClose}
          className="absolute right-4 top-4 rounded-md p-1 text-gray-400 transition-colors hover:text-gray-200"
        >
          <svg className="h-5 w-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>

        <h2 className="mb-5 text-lg font-semibold text-gray-100">New Work Request</h2>

        {/* Error message */}
        {error && (
          <div className="mb-4 rounded-lg border border-red-500/30 bg-red-500/10 px-4 py-3 text-sm text-red-400">
            {error}
          </div>
        )}

        <form onSubmit={handleSubmit} className="space-y-4">
          {/* 1. Title */}
          <div>
            <label htmlFor="create-title" className={labelClass}>Title</label>
            <input
              id="create-title"
              name="title"
              value={form.title}
              onChange={handleChange}
              placeholder="e.g. Redesign Landing Page"
              className={inputClass}
            />
          </div>

          {/* 2. Client Name */}
          <div>
            <label htmlFor="create-client" className={labelClass}>Client Name</label>
            <input
              id="create-client"
              name="clientName"
              value={form.clientName}
              onChange={handleChange}
              placeholder="e.g. Acme Corp"
              className={inputClass}
            />
          </div>

          {/* 3. Description */}
          <div>
            <label htmlFor="create-desc" className={labelClass}>Description</label>
            <textarea
              id="create-desc"
              name="description"
              value={form.description}
              onChange={handleChange}
              rows={3}
              placeholder="Describe the work request..."
              className={`${inputClass} resize-none`}
            />
          </div>

          {/* 4 & 5. Priority + Status (side by side) */}
          <div className="grid grid-cols-2 gap-3">
            <div>
              <label htmlFor="create-priority" className={labelClass}>Priority</label>
              <select
                id="create-priority"
                name="priority"
                value={form.priority}
                onChange={handleChange}
                className={inputClass}
              >
                <option value="Low">Low</option>
                <option value="Medium">Medium</option>
                <option value="High">High</option>
              </select>
            </div>
            <div>
              <label htmlFor="create-status" className={labelClass}>Status</label>
              <select
                id="create-status"
                name="status"
                value={form.status}
                onChange={handleChange}
                className={inputClass}
              >
                <option value="New">New</option>
                <option value="InProgress">In Progress</option>
                <option value="Blocked">Blocked</option>
                <option value="Completed">Completed</option>
              </select>
            </div>
          </div>

          {/* 6. Due Date */}
          <div>
            <label htmlFor="create-due" className={labelClass}>Due Date</label>
            <input
              id="create-due"
              type="date"
              name="dueDate"
              value={form.dueDate}
              onChange={handleChange}
              min={todayStr}
              className={inputClass}
            />
          </div>

          {/* Actions */}
          <div className="flex items-center justify-between pt-2">
            <button
              type="button"
              onClick={handleClose}
              className="rounded-lg px-4 py-2.5 text-sm font-medium text-gray-400 transition-colors hover:text-gray-200"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={isSubmitting}
              className="flex items-center gap-2 rounded-lg bg-indigo-600 px-5 py-2.5 text-sm font-medium text-white transition-colors hover:bg-indigo-500 disabled:cursor-not-allowed disabled:opacity-50"
            >
              {isSubmitting ? (
                <>
                  <svg className="h-4 w-4 animate-spin" fill="none" viewBox="0 0 24 24">
                    <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" />
                    <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z" />
                  </svg>
                  Creating...
                </>
              ) : (
                'Create Request'
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}
