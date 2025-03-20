import { useState } from 'react';
import { PencilIcon, TrashIcon } from '@heroicons/react/24/outline';

interface InventoryItemProps {
  id: string;
  name: string;
  quantity: number;
  price: number;
  category: string;
  supplier: string;
  onEdit: (id: string) => void;
  onDelete: (id: string) => void;
}

export default function InventoryItem({
  id,
  name,
  quantity,
  price,
  category,
  supplier,
  onEdit,
  onDelete,
}: InventoryItemProps) {
  const [isHovered, setIsHovered] = useState(false);

  return (
    <div
      className="bg-white shadow rounded-lg p-4 mb-4 transition-all duration-200 hover:shadow-lg"
      onMouseEnter={() => setIsHovered(true)}
      onMouseLeave={() => setIsHovered(false)}
    >
      <div className="flex justify-between items-start">
        <div>
          <h3 className="text-lg font-semibold text-gray-900">{name}</h3>
          <p className="text-sm text-gray-500">{category}</p>
        </div>
        {isHovered && (
          <div className="flex space-x-2">
            <button
              onClick={() => onEdit(id)}
              className="p-1 text-gray-400 hover:text-indigo-600"
            >
              <PencilIcon className="h-5 w-5" />
            </button>
            <button
              onClick={() => onDelete(id)}
              className="p-1 text-gray-400 hover:text-red-600"
            >
              <TrashIcon className="h-5 w-5" />
            </button>
          </div>
        )}
      </div>
      <div className="mt-4 grid grid-cols-3 gap-4">
        <div>
          <p className="text-sm font-medium text-gray-500">Quantity</p>
          <p className="mt-1 text-sm text-gray-900">{quantity}</p>
        </div>
        <div>
          <p className="text-sm font-medium text-gray-500">Price</p>
          <p className="mt-1 text-sm text-gray-900">${price.toFixed(2)}</p>
        </div>
        <div>
          <p className="text-sm font-medium text-gray-500">Supplier</p>
          <p className="mt-1 text-sm text-gray-900">{supplier}</p>
        </div>
      </div>
      {quantity <= 10 && (
        <div className="mt-4">
          <p className="text-sm text-red-600">Low stock alert!</p>
        </div>
      )}
    </div>
  );
} 