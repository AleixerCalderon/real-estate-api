import { PropertyFilter } from '@/types';
import { Search, Filter, X, DollarSign, MapPin } from 'lucide-react';
import { useState } from 'react';

interface SearchFiltersProps {
  filters: PropertyFilter;
  onFiltersChange: (filters: PropertyFilter) => void;
  onSearch: () => void;
  onClear: () => void;
  loading?: boolean;
}

export function SearchFilters({ 
  filters, 
  onFiltersChange, 
  onSearch, 
  onClear, 
  loading = false 
}: SearchFiltersProps) {
  const [isExpanded, setIsExpanded] = useState(false);

  const handleInputChange = (field: keyof PropertyFilter, value: string | number) => {
    onFiltersChange({
      ...filters,
      [field]: value === '' ? undefined : value,
    });
  };

  const handleKeyPress = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter') {
      onSearch();
    }
  };

  return (
    <div className="bg-white p-6 rounded-lg shadow-md mb-6 animate-slide-up">
      <div className="flex items-center gap-4 mb-4">
        <div className="flex-1">
          <div className="relative">
            <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 h-4 w-4" />
            <input
              type="text"
              placeholder="Buscar por nombre de propiedad..."
              value={filters.name || ''}
              onChange={(e) => handleInputChange('name', e.target.value)}
              onKeyPress={handleKeyPress}
              disabled={loading}
              className="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all disabled:opacity-50"
            />
          </div>
        </div>
        
        <button
          onClick={() => setIsExpanded(!isExpanded)}
          className={`flex items-center gap-2 px-4 py-3 border rounded-lg transition-all ${
            isExpanded 
              ? 'bg-blue-50 border-blue-300 text-blue-700' 
              : 'text-gray-600 border-gray-300 hover:bg-gray-50'
          }`}
        >
          <Filter className="h-4 w-4" />
          Filtros Avanzados
        </button>
      </div>

      {isExpanded && (
        <div className="grid grid-cols-1 md:grid-cols-3 gap-4 mb-4 animate-slide-up">
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              <MapPin className="inline h-4 w-4 mr-1" />
              Dirección
            </label>
            <input
              type="text"
              placeholder="Buscar por dirección..."
              value={filters.address || ''}
              onChange={(e) => handleInputChange('address', e.target.value)}
              onKeyPress={handleKeyPress}
              disabled={loading}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all disabled:opacity-50"
            />
          </div>
          
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              <DollarSign className="inline h-4 w-4 mr-1" />
              Precio Mínimo
            </label>
            <input
              type="number"
              placeholder="0"
              value={filters.minPrice || ''}
              onChange={(e) => handleInputChange('minPrice', Number(e.target.value))}
              onKeyPress={handleKeyPress}
              disabled={loading}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all disabled:opacity-50"
            />
          </div>
          
          <div>
            <label className="block text-sm font-medium text-gray-700 mb-2">
              <DollarSign className="inline h-4 w-4 mr-1" />
              Precio Máximo
            </label>
            <input
              type="number"
              placeholder="999999999"
              value={filters.maxPrice || ''}
              onChange={(e) => handleInputChange('maxPrice', Number(e.target.value))}
              onKeyPress={handleKeyPress}
              disabled={loading}
              className="w-full px-3 py-2 border border-gray-300 rounded-md focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all disabled:opacity-50"
            />
          </div>
        </div>
      )}

      <div className="flex items-center gap-3 flex-wrap">
        <button
          onClick={onSearch}
          disabled={loading}
          className="bg-blue-600 text-white px-6 py-2 rounded-lg hover:bg-blue-700 transition-colors flex items-center gap-2 disabled:opacity-50 disabled:cursor-not-allowed"
        >
          <Search className="h-4 w-4" />
          {loading ? 'Buscando...' : 'Buscar Propiedades'}
        </button>
        
        <button
          onClick={onClear}
          disabled={loading}
          className="bg-gray-500 text-white px-4 py-2 rounded-lg hover:bg-gray-600 transition-colors flex items-center gap-2 disabled:opacity-50"
        >
          <X className="h-4 w-4" />
          Limpiar Filtros
        </button>

        {(filters.name || filters.address || filters.minPrice || filters.maxPrice) && (
          <div className="text-sm text-gray-600 bg-blue-50 px-3 py-1 rounded-full">
            Filtros activos
          </div>
        )}
      </div>
    </div>
  );
}