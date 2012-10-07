class RemoveIntegerFromLineItems < ActiveRecord::Migration
  def up
    remove_column :line_items, :integer
      end

  def down
    add_column :line_items, :integer, :string
  end
end
