class CreateLineItems < ActiveRecord::Migration
  def change
    create_table :line_items do |t|
      t.string :product_id
      t.string :integer
      t.string :cart_id
      t.string :integer

      t.timestamps
    end
  end
end
